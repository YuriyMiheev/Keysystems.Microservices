﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microservices.Channels.Configuration;
using Microservices.Channels.Data;
using Microservices.Channels.Logging;
using Microservices.Channels.MSSQL.Adapters;
using Microservices.Channels.MSSQL.Configuration;
using Microservices.Channels.MSSQL.Data;

namespace Microservices.Channels.MSSQL
{
	public class ChannelService : IChannelService, IDisposable
	{
		private ChannelDatabase _database;
		private MessageReceiver _receiver;
		//private MessageSender _sender;
		//private MessagePublisher _publisher;
		private List<SendMessageScanner> _scanSenders;
		private CancellationTokenSource _cancellationSource;


		#region Ctor
		public ChannelService(ChannelConfigFileSettings channelSettings, ServiceConfigFileSettings serviceSettings)
		{
			_channelSettings = channelSettings ?? throw new ArgumentNullException(nameof(channelSettings));
			_serviceSettings = serviceSettings ?? throw new ArgumentNullException(nameof(serviceSettings));

			_cancellationSource = new CancellationTokenSource();

			_receiver = new MessageReceiver(this);
			_scanSenders = new List<SendMessageScanner>();
		}
		#endregion


		#region Settings
		private ChannelConfigFileSettings _channelSettings;
		/// <summary>
		/// {Get} Настройки канала.
		/// </summary>
		public ChannelConfigFileSettings ChannelSettings
		{
			get { return _channelSettings; }
		}

		private ServiceConfigFileSettings _serviceSettings;
		/// <summary>
		/// {Get} Настройки сервиса.
		/// </summary>
		public ServiceConfigFileSettings ServiceSettings
		{
			get { return _serviceSettings; }
		}

		private DatabaseSettings _databaseSettings;
		/// <summary>
		/// {Get} Настройки для работы с БД.
		/// </summary>
		public DatabaseSettings DatabaseSettings
		{
			get { return _databaseSettings; }
		}

		private MessageSettings _messageSettings;
		/// <summary>
		/// {Get} Настройки обработки сообщений.
		/// </summary>
		public MessageSettings MessageSettings
		{
			get { return _messageSettings; }
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Виртуальный адрес канала.
		/// </summary>
		public string VirtAddress
		{
			get { return _channelSettings.VirtAddress; }
		}

		private MessageDataAdapter _dataAdapter;
		/// <summary>
		/// {Get} Адаптер БД сообщений.
		/// </summary>
		public MessageDataAdapter MessageDataAdapter
		{
			get { return _dataAdapter; }
		}

		public bool Opened { get; private set; }

		public bool Running { get; private set; }

		public bool? Online { get; private set; }
		#endregion


		#region Control
		public void Open()
		{
			Initialize();

			//_receiver = new MessageReceiver(this);
			//_scanSenders = new List<SendMessageScanner>();

			//_sender = new MessageSender(this);
			//_publisher = new MessagePublisher(this);
			//_scanSenders = new List<SendMessageProcessor>();
			//_scanPublisher = new PublishMessageProcessor(this.MessageService, THIS, this.MessageDataAdapter, "*");
			//_scanSubscriber = new SubscribeMessageProcessor(this.MessageService, THIS);

			this.Opened = true;
			//UpdateMyselfContact(this.Info);

			//ServiceInfo serviceInfo = this.MessageService.GetInfo();
			//if (serviceInfo.ChannelsSettings.CheckDatabaseUsed)
			//{
			//	ChannelInfo existChannel;
			//	if (TryCheckDatabaseUsedOtherChannel(out existChannel))
			//	{
			//		if (existChannel.LINK != this.LINK)
			//		{
			//			var error = new ChannelException(this, String.Format("БД уже используется другим каналом: {0}.", existChannel));
			//			error.ErrorCode = ChannelException.DatabaseUsedOtherChannel;
			//			error.Data.Add("OtherChannelInfo", existChannel.ToDto());

			//			throw error;
			//		}
			//	}

			//	Uri rmsUri;
			//	Contact contact;
			//	if (TryCheckDatabaseUsedOtherRms(out rmsUri, out contact))
			//	{
			//		var error = new ChannelException(this, String.Format("БД уже используется другим RMS сервисом: {0}.", rmsUri.Host));
			//		error.ErrorCode = ChannelException.DatabaseUsedOtherRms;
			//		error.Data.Add("OtherRmsUri", rmsUri);
			//		if (contact != null)
			//			error.Data.Add("OtherRmsContact", contact.ToDto());

			//		throw error;
			//	}
			//}
		}

		public void Run()
		{
			Exception error;
			if (!TryConnect(out error))
				throw error;

			if (_messageSettings.DeleteDeleted)
			{
				Task.Factory.StartNew(() =>
					{
						try
						{
							string sql = $"DELETE FROM {Database.Tables.MESSAGES} WHERE STATUS='{MessageStatus.DELETED}'";
							int count = _dataAdapter.ExecuteUpdate(sql);

							LogTrace($"Удалено удаленных сообщений: {count}");
						}
						catch (Exception ex)
						{
							LogError(ex);
						}
					}, TaskCreationOptions.LongRunning);
			}

			Task.Factory.StartNew(() =>
				{
					try
					{
						string statusInfo = new ChannelException(this, "Отправка сообщения была прервана.").ToString();
						string sql = $"UPDATE {Database.Tables.MESSAGES} SET STATUS='{MessageStatus.ERROR}', STATUS_INFO='{statusInfo}' WHERE DIRECTION='{MessageDirection.OUT}' AND STATUS='{MessageStatus.SENDING}'";
						int count = _dataAdapter.ExecuteUpdate(sql);

						LogTrace($"Найдено отправленных недоставленных сообщений: {count}");
					}
					catch (Exception ex)
					{
						LogError(ex);
					}
				}, TaskCreationOptions.LongRunning);

			Task.Factory.StartNew(() =>
				{
					try
					{
						string statusInfo = new ChannelException(this, "Прием сообщения был прерван.").ToString();
						string sql = $"UPDATE {Database.Tables.MESSAGES} SET STATUS='{MessageStatus.ERROR}', STATUS_INFO='{statusInfo}' WHERE DIRECTION='{MessageDirection.IN}' AND STATUS='{MessageStatus.SENDING}'";
						int count = _dataAdapter.ExecuteUpdate(sql);

						LogTrace($"Найдено непринятых входящих сообщений: {count}");
					}
					catch (Exception ex)
					{
						LogError(ex);
					}
				}, TaskCreationOptions.LongRunning);


			if (_messageSettings.ScanEnabled)
			{
				List<string> recipients = _dataAdapter.GetAllRecipients()
					.Where(address => MessageValidator.IsAddressValid(address)).ToList();

				if (recipients.Count > 0)
					LogTrace(String.Format("Среди сообщений найдено уникальных получателей {0}: {1}.", recipients.Count, String.Join(", ", recipients)));

				//	if (recipients.Count == 0)
				//		recipients.Add("*");

				List<string> existRecipients = _scanSenders.Select(scanner => scanner.Recipient).ToList();
				List<string> newRecipients = recipients.Except(existRecipients).ToList();

				newRecipients.ForEach(recipient =>
					{
						var scanner = new SendMessageScanner(this, recipient);
						_scanSenders.Add(scanner);
					});

				_scanSenders.ForEach(sender =>
					{
						sender.Start(this.MessageSettings.ScanInterval, new ActionBlock<Message>(null), _cancellationSource.Token);
					});

				//	if (this.MessageService.ChannelManager.GetSubscribers(this.LINK).Count > 0)
				//		this.scanPublisher.Start(this.MessageSettings.ScanThreads, this.cancelToken);

				//	if (this.MessageService.ChannelManager.GetPublishers(this.LINK).Count > 0)
				//		this.scanSubscriber.Start(this.MessageSettings.ScanThreads, this.cancelToken);
			}

			this.Running = true;

			//UpdateMyselfContact(this.Info);
		}

		public void Stop()
		{
			//OnStopping();

			//try
			//{
			//	List<Task> completions = _scanSenders.Select(sender => sender.Completion).ToList();
			//	completions.Add(_scanPublisher.Completion);
			//	completions.AddRange(_scanSubscriber.Completion);
			//	Task[] notCompletions = completions.Where(task => task != null).Where(task => !task.IsCompleted && !task.IsCanceled && !task.IsFaulted).ToArray();

			//	Task.WaitAll(notCompletions);
			//}
			//catch
			//{ }
			//finally
			//{
			//	//this.Running = false;
			//	//this.Online = null;
			//}

			//UpdateMyselfContact(this.Info);
		}

		public void Close()
		{
			this.Opened = false;
			this.Running = false;
			this.Online = null;

			//UpdateMyselfContact(myInfo);

			Dispose();
		}
		#endregion


		#region Diagnostic
		public bool TryConnect(out Exception error)
		{
			Initialize();

			ConnectionException ex;
			if (_database.TryConnect(out ex))
			{
				error = null;
				this.Online = true;
				return true;
			}
			else
			{
				error = ex;
				this.Online = false;
				return false;
			}
		}

		/// <summary>
		/// Запустить диагностику канала.
		/// </summary>
		public void CheckState()
		{
			Initialize();

			_database.ValidateSchema();
		}

		/// <summary>
		/// Восстановить работоспособность канала.
		/// </summary>
		public void Repair()
		{
			Initialize();

			if (this.DatabaseSettings.RepairSPEnabled)
			{
				try
				{
					string repairSP = this.DatabaseSettings.RepairSP;
					if (String.IsNullOrWhiteSpace(repairSP))
						throw new InvalidOperationException("Не указано имя хранимой процедуры восстановления БД.");

					LogTrace(String.Format("Вызов хранимой процедуры \"{0}\".", repairSP));
					_dataAdapter.CallRepairSP(repairSP);
				}
				catch (Exception ex)
				{
					SetError(ex);
					LogError(ex);
				}
			}

			_database.CreateOrUpdateSchema();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Ping()
		{
			Initialize();

			Exception error;
			if (!TryConnect(out error))
				throw error;

			if (this.DatabaseSettings.PingSPEnabled)
			{
				string pingSP = this.DatabaseSettings.PingSP;
				if (String.IsNullOrWhiteSpace(pingSP))
					throw new ConfigSettingsException("Не указано имя хранимой процедуры пинга БД.", "DATABASE.PING_SP");

				LogTrace(String.Format("Вызов хранимой процедуры \"{0}\".", pingSP));
				_dataAdapter.CallPingSP(pingSP);
			}
		}
		#endregion


		#region Error
		/// <summary>
		/// Сбросить ошибку.
		/// </summary>
		public void ClearError()
		{
		}

		/// <summary>
		/// Запомнить ошибку.
		/// </summary>
		/// <param name="error"></param>
		public void SetError(Exception error)
		{
		}

		/// <summary>
		/// Вызвать ошибку.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public ChannelException ThrowError(string text)
		{
			return new ChannelException(this, text);
		}
		#endregion


		#region IMessageRepository
		/// <summary>
		/// 
		/// </summary>
		/// <param name="queryParams"></param>
		/// <returns></returns>
		public List<Message> SelectMessages(QueryParams queryParams)
		{
			#region Validate parameters
			if (queryParams == null)
				throw new ArgumentNullException("queryParams");
			#endregion

			return _dataAdapter.SelectMessages(queryParams);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="status"></param>
		/// <param name="skip"></param>
		/// <param name="take"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		public List<Message> GetMessages(string status, int? skip, int? take, out int totalCount)
		{
			return _dataAdapter.GetMessages("*", status, skip, take, out totalCount);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="status"></param>
		/// <param name="skip"></param>
		/// <param name="take"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		public List<Message> GetLastMessages(string status, int? skip, int? take, out int totalCount)
		{
			return _dataAdapter.GetLastMessages("*", status, skip, take, out totalCount);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		/// <returns></returns>
		public Message GetMessage(int msgLink)
		{
			Message msg = _dataAdapter.GetMessage(msgLink);
			if (msg == null)
				throw new MessageNotFoundException(msgLink);

			return msg;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		/// <returns></returns>
		public Message FindMessage(int msgLink)
		{
			return _dataAdapter.GetMessage(msgLink);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgGuid"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public Message FindMessage(string msgGuid, string direction)
		{
			return _dataAdapter.FindMessage("*", msgGuid, direction);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		public void SaveMessage(Message msg)
		{
			PrepareSaveMessage(msg);
			_dataAdapter.SaveMessage(msg);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		public void DeleteMessage(int msgLink)
		{
			_dataAdapter.DeleteMessage(msgLink);
		}

		/// <summary>
		/// Удалить устаревшие сообщения.
		/// </summary>
		/// <param name="expiredDate"></param>
		/// <param name="statuses"></param>
		public void DeleteExpiredMessages(DateTime expiredDate, List<string> statuses)
		{
			_dataAdapter.DeleteExpiredMessages("*", expiredDate, statuses);
		}

		/// <summary>
		/// Удалить устаревшие сообщения.
		/// </summary>
		/// <param name="msgLinks"></param>
		public void DeleteMessages(IEnumerable<int> msgLinks)
		{
			_dataAdapter.DeleteMessages(msgLinks);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="begin"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public List<DAO.DateStatMessage> GetMessagesByDate(DateTime? begin, DateTime? end)
		{
			return _dataAdapter.GetMessagesByDate("*", begin, end);
		}


		#region Body
		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		/// <returns></returns>
		public MessageBody GetMessageBody(int msgLink)
		{
			MessageBody body = _dataAdapter.GetMessageBody(msgLink);
			if (body == null)
				throw new MessageBodyNotFoundException(msgLink);

			return body;
		}

		/// <summary>
		/// Сохранить тело сообщения.
		/// </summary>
		/// <param name="body"></param>
		public virtual void SaveMessageBody(MessageBody body)
		{
			_dataAdapter.SaveMessageBody(body);
		}

		/// <summary>
		/// Удалить тело сообщения.
		/// </summary>
		/// <param name="msgLink"></param>
		public virtual void DeleteMessageBody(int msgLink)
		{
			_dataAdapter.DeleteMessageBody(msgLink);
		}
		#endregion


		#region Content
		/// <summary>
		/// Получить контент сообщения.
		/// </summary>
		/// <param name="contentLink"></param>
		/// <returns></returns>
		public virtual MessageContent GetMessageContent(int contentLink)
		{
			MessageContent content = _dataAdapter.GetMessageContent(contentLink);
			if (content == null)
				throw new MessageContentNotFoundException(contentLink, contentLink);

			return content;
		}

		/// <summary>
		/// Сохранить контент сообщения.
		/// </summary>
		/// <param name="content"></param>
		public virtual void SaveMessageContent(MessageContent content)
		{
			_dataAdapter.SaveMessageContent(content);
		}

		/// <summary>
		/// Удалить контент сообщения.
		/// </summary>
		/// <param name="contentLink"></param>
		public virtual void DeleteMessageContent(int contentLink)
		{
			_dataAdapter.DeleteMessageContent(contentLink);
		}
		#endregion

		#endregion


		#region Messages
		///// <summary>
		///// Отправить сообщение.
		///// </summary>
		///// <param name="msgLink"></param>
		///// <returns></returns>
		//public int? SendMessage(int msgLink)
		//{
		//	Message outMsg = GetMessage(msgLink);
		//	Message resMsg = _sender.SendMessage(outMsg);
		//	int? resLink = (resMsg != null ? resMsg.LINK : new Nullable<int>());

		//	if (_messageSettings.DeleteAfterSend)
		//	{
		//		if (outMsg.TTL == null)
		//			outMsg.TTL = DateTime.Now;

		//		outMsg.SetStatus(MessageStatus.DELETED, "Удалено после отправки.");
		//		SaveMessage(outMsg);
		//	}

		//	return resLink;
		//}

		///// <summary>
		///// Отправить сообщение асинхронно.
		///// </summary>
		///// <param name="msgLink"></param>
		//public void SendMessageAsync(int msgLink)
		//{
		//	Message msg = GetMessage(msgLink);
		//	//_sender.SendMessageAsync(msg);
		//}

		//public Message PreSendMessage(int msgLink)
		//{
		//	Message msg = GetMessage(msgLink);
		//	return msg;
		//}

		///// <summary>
		///// Опубликовать сообщение.
		///// </summary>
		///// <param name="msgLink"></param>
		//public void PublishMessage(int msgLink)
		//{
		//	Message msg = GetMessage(msgLink);
		//	//_publisher.PublishMessage(msg);
		//}

		/// <summary>
		/// Принять сообщение.
		/// </summary>
		/// <param name="msgLink"></param>
		/// <returns></returns>
		public int? ReceiveMessage(int msgLink)
		{
			Message inMsg = GetMessage(msgLink);
			Message resMsg = _receiver.ReceiveMessage(inMsg);

			if (_messageSettings.DeleteAfterReceive)
			{
				try
				{
					//if (resMsg.TTL == null)
					//	resMsg.TTL = DateTime.Now;

					resMsg.SetStatus(MessageStatus.DELETED, "Удалено после приема.");
					SaveMessage(resMsg);
				}
				catch (Exception ex)
				{
					LogTrace(ex.ToString());
				}
			}

			return (resMsg != null ? resMsg.LINK : new Nullable<int>());
		}
		#endregion


		#region ILogger
		/// <summary>
		/// 
		/// </summary>
		void ILogger.InitializeLogger()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="error"></param>
		public void LogError(Exception error)
		{
			#region Validate parameters
			if (error == null)
				throw new ArgumentNullException("error");
			#endregion

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="error"></param>
		public void LogError(string text, Exception error)
		{
			#region Validate parameters
			if (error == null)
				throw new ArgumentNullException("error");
			#endregion

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		public void LogInfo(string text)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		public void LogTrace(string text)
		{
		}
		#endregion


		#region Helper
		private bool _initialized;
		private void Initialize()
		{
			if (!_initialized)
			{
				lock (this)
				{
					if (!_initialized)
					{
						_initialized = true;

						try
						{
							IDictionary<string, ConfigFileSetting> appSettings = _channelSettings.GetAppSettings();
							_databaseSettings = new DatabaseSettings(appSettings);
							_messageSettings = new MessageSettings(appSettings);

							_database = new ChannelDatabase();
							_database.Schema = _databaseSettings.Schema;
							_database.ConnectionString = _channelSettings.RealAddress;

							DbContext dbContext = _database.ValidateSchema();
							dbContext.ConnectionChanged += (s, e) => { };

							_dataAdapter = new MessageDataAdapter(dbContext);
							_dataAdapter.ExecuteTimeout = (int)_databaseSettings.ExecuteTimeout.TotalSeconds;
						}
						catch
						{
							_initialized = false;
							throw;
						}
					}
				}
			}
		}

		private void PrepareSaveMessage(Message msg)
		{
			msg.Channel = this.VirtAddress;

			if (String.IsNullOrWhiteSpace(msg.Version))
				msg.Version = MessageVersion.Current;

			if (String.IsNullOrWhiteSpace(msg.Type))
				msg.Type = MessageType.DOCUMENT;

			if (msg.Date == null)
				msg.Date = DateTime.Now;

			if (msg.LINK == 0)
				msg.SetStatus(MessageStatus.NULL);
		}
		#endregion


		#region IDisposable
		private bool _disposed = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					_cancellationSource.Cancel();
					//OnClosing();

					//this.Stopping = null;
					//this.Closing = null;

					if (_dataAdapter != null)
						_dataAdapter.DbContext.Dispose();

					//if (_sender != null)
					//	_sender.Dispose();

					//if (_publisher != null)
					//	_publisher.Dispose();

					//if (_scanSenders != null)
					//	_scanSenders.ForEach(sender => sender.Dispose());

					//if (_scanPublisher != null)
					//	_scanPublisher.Dispose();

					//if (_scanSubscriber != null)
					//	_scanSubscriber.Dispose();

					_database.Close();

					this.Opened = false;
					this.Running = false;
					this.Online = null;
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				_disposed = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~ChannelService()
		// {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion

	}
}
