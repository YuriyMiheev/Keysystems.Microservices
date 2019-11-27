﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Channels.Adapters;
using Microservices.Channels.Configuration;
using Microservices.Channels.Data;
using Microservices.Channels.Logging;
using Microservices.Channels.MSSQL.Adapters;
using Microservices.Channels.MSSQL.Data;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using Microservices.Channels.MSSQL.Hubs;
using Microservices.Channels.Hubs;

namespace Microservices.Channels.MSSQL
{
	public class ChannelService : IChannelService, IDisposable
	{
		//private IServiceProvider _serviceProvider;
		private IDatabase _database;
		private XmlConfigFileConfigurationProvider _appConfig;
		private CancellationTokenSource _cancellationSource;
		private bool _initialized;
		//private ChannelDatabase _database;
		//private MessagePublisher _publisher;
		private MessageReceiver _receiver;
		private ISendMessageScanner _sender;
		private ILogger _logger;


		#region Ctor
		public ChannelService(IServiceProvider serviceProvider)
		{
			//_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			_appConfig = serviceProvider.GetRequiredService<XmlConfigFileConfigurationProvider>();
			_logger = serviceProvider.GetRequiredService<ILogger>();
			_database = serviceProvider.GetRequiredService<IDatabase>();
			_sender = serviceProvider.GetRequiredService<ISendMessageScanner>();

			_cancellationSource = new CancellationTokenSource();

			IDictionary<string, ConfigFileSetting> appSettings = _appConfig.AppSettings;
			_infoSettings = new InfoSettings(appSettings);
			_channelSettings = new ChannelSettings(appSettings);
			_databaseSettings = new DatabaseSettings(appSettings);
			_messageSettings = new MessageSettings(appSettings);
			_serviceSettings = new ServiceSettings(appSettings);

			_receiver = new MessageReceiver(this, _logger);
			//_publisher = new MessagePublisher(this);

			_processId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
		}
		#endregion


		private MessageDataAdapter _dataAdapter;
		/// <summary>
		/// {Get} Адаптер БД сообщений.
		/// </summary>
		public MessageDataAdapterBase MessageDataAdapter
		{
			get { return _dataAdapter; }
		}


		#region Events
		/// <summary>
		/// 
		/// </summary>
		public event Func<Message[], bool> OutMessages;
		#endregion


		#region Settings
		private InfoSettings _infoSettings;
		/// <summary>
		/// {Get} Общие настройки.
		/// </summary>
		public InfoSettings InfoSettings
		{
			get { return _infoSettings; }
		}

		private ChannelSettings _channelSettings;
		/// <summary>
		/// {Get} Настройки канала.
		/// </summary>
		public ChannelSettings ChannelSettings
		{
			get { return _channelSettings; }
		}

		private DatabaseSettings _databaseSettings;
		/// <summary>
		/// {Get} Настройки БД.
		/// </summary>
		public DatabaseSettings DatabaseSettings
		{
			get { return _databaseSettings; }
		}

		private MessageSettings _messageSettings;
		/// <summary>
		/// {Get} Настройки сообщений.
		/// </summary>
		public MessageSettings MessageSettings
		{
			get { return _messageSettings; }
		}

		private ServiceSettings _serviceSettings;
		/// <summary>
		/// {Get} Настройки сервиса.
		/// </summary>
		public ServiceSettings ServiceSettings
		{
			get { return _serviceSettings; }
		}

		public IDictionary<string, ConfigFileSetting> GetAppSettings()
		{
			return _appConfig.AppSettings;
		}

		public void SetAppSettings(IDictionary<string, string> settings)
		{
			foreach (string key in settings.Keys)
			{
				if (_appConfig.AppSettings.ContainsKey(key))
					_appConfig.AppSettings[key].Value = settings[key];
			}
		}

		public void SaveAppSettings()
		{
		}
		#endregion


		#region Properties
		private string _processId;
		/// <summary>
		/// {Get}
		/// </summary>
		public string ProcessId
		{
			get { return _processId; }
		}

		/// <summary>
		/// {Get} Виртуальный адрес канала.
		/// </summary>
		public string VirtAddress
		{
			get { return _infoSettings.VirtAddress; }
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public bool Opened { get; private set; }

		/// <summary>
		/// {Get}
		/// </summary>
		public bool Running { get; private set; }

		/// <summary>
		/// {Get}
		/// </summary>
		public bool? Online { get; private set; }
		#endregion


		#region Control
		public void Open()
		{
			Initialize();

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
			CheckOpened();

			if (_cancellationSource.IsCancellationRequested)
				_cancellationSource = new CancellationTokenSource();

			Exception error;
			if (!TryConnect(out error))
				throw error;

			if (_messageSettings.DeleteDeleted)
			{
				try
				{
					string sql = $"DELETE FROM {Database.Tables.MESSAGES} WHERE STATUS='{MessageStatus.DELETED}'";
					int count = _dataAdapter.ExecuteUpdate(sql);

					LogTrace($"Удалено сообщений: {count}");
				}
				catch (Exception ex)
				{
					LogError(ex);
				}
			}

			try
			{
				string statusInfo = new ChannelException(this, "Отправка сообщения была прервана.").ToString();
				string sql = $"UPDATE {Database.Tables.MESSAGES} SET STATUS='{MessageStatus.ERROR}', STATUS_INFO='{statusInfo}', STATUS_DATE='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}' WHERE DIRECTION='{MessageDirection.OUT}' AND STATUS='{MessageStatus.SENDING}'";
				int count = _dataAdapter.ExecuteUpdate(sql);

				LogTrace($"Найдено недоставленных сообщений: {count}");
			}
			catch (Exception ex)
			{
				LogError(ex);
			}

			try
			{
				string statusInfo = new ChannelException(this, "Прием сообщения был прерван.").ToString();
				string sql = $"UPDATE {Database.Tables.MESSAGES} SET STATUS='{MessageStatus.ERROR}', STATUS_INFO='{statusInfo}', STATUS_DATE='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}' WHERE DIRECTION='{MessageDirection.IN}' AND STATUS='{MessageStatus.RECEIVING}'";
				int count = _dataAdapter.ExecuteUpdate(sql);

				LogTrace($"Найдено непринятых сообщений: {count}");
			}
			catch (Exception ex)
			{
				LogError(ex);
			}


			if (_messageSettings.ScanEnabled)
			{
				_sender.Start(_cancellationSource.Token);

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
			_cancellationSource.Token.Register(() =>
				{
					this.Running = false;
					this.Online = null;
				});
			_cancellationSource.Cancel();
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
			//}

			//UpdateMyselfContact(this.Info);
		}

		public void Close()
		{
			_cancellationSource.Token.Register(() =>
				{
					this.Opened = false;
					//this.Running = false;
					//this.Online = null;
				});

			try
			{
				Stop();
				//UpdateMyselfContact(myInfo);
			}
			finally
			{
				Dispose();
			}
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

			if (_databaseSettings.RepairSPEnabled)
			{
				try
				{
					string repairSP = _databaseSettings.RepairSP;
					if (String.IsNullOrWhiteSpace(repairSP))
						throw new InvalidOperationException("Не указано имя хранимой процедуры восстановления БД.");

					LogTrace($"Вызов хранимой процедуры \"{repairSP}\".");
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

			if (_databaseSettings.PingSPEnabled)
			{
				string pingSP = _databaseSettings.PingSP;
				if (String.IsNullOrWhiteSpace(pingSP))
					throw new InvalidOperationException("Не указано имя хранимой процедуры пинга БД.");

				LogTrace($"Вызов хранимой процедуры \"{pingSP}\".");
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


		#region Messages
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

			CheckOpened();
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
			CheckOpened();
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
			CheckOpened();
			return _dataAdapter.GetLastMessages("*", status, skip, take, out totalCount);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		/// <returns></returns>
		public Message GetMessage(int msgLink)
		{
			CheckOpened();

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
			CheckOpened();
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
			CheckOpened();
			return _dataAdapter.FindMessage("*", msgGuid, direction);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		public void SaveMessage(Message msg)
		{
			CheckOpened();
			PrepareSaveMessage(msg);
			_dataAdapter.SaveMessage(msg);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		public void DeleteMessage(int msgLink)
		{
			CheckOpened();
			_dataAdapter.DeleteMessage(msgLink);
		}

		/// <summary>
		/// Удалить устаревшие сообщения.
		/// </summary>
		/// <param name="expiredDate"></param>
		/// <param name="statuses"></param>
		public void DeleteExpiredMessages(DateTime expiredDate, List<string> statuses)
		{
			CheckOpened();
			_dataAdapter.DeleteExpiredMessages("*", expiredDate, statuses);
		}

		/// <summary>
		/// Удалить устаревшие сообщения.
		/// </summary>
		/// <param name="msgLinks"></param>
		public void DeleteMessages(IEnumerable<int> msgLinks)
		{
			CheckOpened();
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
			CheckOpened();
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
			CheckOpened();
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
			CheckOpened();
			_dataAdapter.SaveMessageBody(body);
		}

		/// <summary>
		/// Удалить тело сообщения.
		/// </summary>
		/// <param name="msgLink"></param>
		public virtual void DeleteMessageBody(int msgLink)
		{
			CheckOpened();
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
			CheckOpened();
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
			CheckOpened();
			_dataAdapter.SaveMessageContent(content);
		}

		/// <summary>
		/// Удалить контент сообщения.
		/// </summary>
		/// <param name="contentLink"></param>
		public virtual void DeleteMessageContent(int contentLink)
		{
			CheckOpened();
			_dataAdapter.DeleteMessageContent(contentLink);
		}
		#endregion


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
			CheckOpened();
			Message inMsg = GetMessage(msgLink);
			Message resMsg = _receiver.ReceiveMessage(inMsg);

			if (_messageSettings.DeleteAfterReceive)
			{
				try
				{
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

		/// <summary>
		/// Отправить сообщение.
		/// </summary>
		/// <param name="msgLink"></param>
		/// <returns></returns>
		public void SendMessage(int msgLink)
		{
			CheckOpened();
			Message outMsg = GetMessage(msgLink);
			//Message resMsg = _sender.SendMessage(outMsg);
			//int? resLink = (resMsg != null ? resMsg.LINK : new Nullable<int>());

			//if (_messageSettings.DeleteAfterSend)
			//{
			//	if (outMsg.TTL == null)
			//		outMsg.TTL = DateTime.Now;

			//	outMsg.SetStatus(MessageStatus.DELETED, "Удалено после отправки.");
			//	SaveMessage(outMsg);
			//}

			//return resLink;
		}
		#endregion


		//#region ILogger
		///// <summary>
		///// 
		///// </summary>
		//void ILogger.InitializeLogger()
		//{ }

		///// <summary>
		///// 
		///// </summary>
		///// <param name="error"></param>
		//public void LogError(Exception error)
		//{
		//	#region Validate parameters
		//	if (error == null)
		//		throw new ArgumentNullException("error");
		//	#endregion

		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="text"></param>
		///// <param name="error"></param>
		//public void LogError(string text, Exception error)
		//{
		//	#region Validate parameters
		//	if (error == null)
		//		throw new ArgumentNullException("error");
		//	#endregion

		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="text"></param>
		//public void LogInfo(string text)
		//{
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="text"></param>
		//public void LogTrace(string text)
		//{
		//}
		//#endregion


		#region IHostedService  
		Task IHostedService.StartAsync(CancellationToken cancellationToken)
		{
			return Task.Run(() =>
				{
					if (_channelSettings.AutoOpen)
					{
						Open();
						if (_channelSettings.AutoRun)
							Run();
					}
				}, cancellationToken);
		}

		Task IHostedService.StopAsync(CancellationToken cancellationToken)
		{
			return Task.Run(() =>
				{
					Close();
				}, cancellationToken);
		}
		#endregion


		#region Helper
		private void Initialize()
		{
			if (!_initialized)
			{
				lock (this)
				{
					if (!_initialized)
					{
						_initialized = true;

						if (_cancellationSource.IsCancellationRequested)
							_cancellationSource = new CancellationTokenSource();

						try
						{
							_database.Schema = _databaseSettings.Schema;
							_database.ConnectionString = _infoSettings.RealAddress;
							DbContext dbContext = _database.Open();
							_dataAdapter = new MessageDataAdapter(dbContext);
							_dataAdapter.ExecuteTimeout = (int)_databaseSettings.ExecuteTimeout.TotalSeconds;

							_sender = new SendMessageScanner(_dataAdapter, _messageSettings);
							_sender.NewMessages += sender_NewMessages;
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

		private void CheckOpened()
		{
			if (!this.Opened)
				throw new InvalidOperationException("Сервис-канал закрыт.");
		}

		private bool sender_NewMessages(Message[] messages)
		{
			if (this.OutMessages != null)
				return this.OutMessages(messages);
			else
				return false;
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

					if (_sender != null)
						_sender.Dispose();

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
