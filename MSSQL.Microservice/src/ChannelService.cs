using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Microservices;
using Microservices.Channels;
using Microservices.Channels.Configuration;
using Microservices.Channels.Data;
using Microservices.Channels.Logging;
using Microservices.Configuration;
using Microservices.Data;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MSSQL.Microservice
{
	/// <summary>
	/// 
	/// </summary>
	public class ChannelService : IChannelService//, IDisposable
	{
		private CancellationTokenSource _cancellationSource;
		private readonly IChannelControl _control;
		private readonly IAppSettingsConfig _appConfig;
		private readonly ILogger _logger;
		private readonly IChannelDataAdapter _dataAdapter;
		private readonly IMessageReceiver _receiver;
		private readonly ChannelStatus _status;


		#region Ctor
		public ChannelService(IAppSettingsConfig appConfig, IChannelControl control, ILogger logger, ChannelStatus status, IChannelDataAdapter dataAdapter, IMessageReceiver receiver)
		{
			_appConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
			_control = control ?? throw new ArgumentNullException(nameof(control));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_status = status ?? throw new ArgumentNullException(nameof(status));
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
			_receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
			_cancellationSource = new CancellationTokenSource();

			MainSettings mainSettings = _appConfig.MainSettings();

			this.ProcessId = Process.GetCurrentProcess().Id;
			this.VirtAddress = mainSettings.VirtAddress;
		}
		#endregion



		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public int ProcessId { get; }

		/// <summary>
		/// {Get} Виртуальный адрес канала.
		/// </summary>
		public string VirtAddress { get; }
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
			return _dataAdapter.GetMessages(status, skip, take, out totalCount);
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
			return _dataAdapter.GetLastMessages(status, skip, take, out totalCount);
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
			return _dataAdapter.FindMessage(msgGuid, direction);
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

		///// <summary>
		///// Удалить устаревшие сообщения.
		///// </summary>
		///// <param name="expiredDate"></param>
		///// <param name="statuses"></param>
		//public void DeleteExpiredMessages(DateTime expiredDate, List<string> statuses)
		//{
		//	CheckOpened();
		//	_dataAdapter.DeleteExpiredMessages("*", expiredDate, statuses);
		//}

		/// <summary>
		/// Удалить устаревшие сообщения.
		/// </summary>
		/// <param name="msgLinks"></param>
		public void DeleteMessages(IEnumerable<int> msgLinks)
		{
			CheckOpened();
			_dataAdapter.DeleteMessages(msgLinks);
		}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="begin"></param>
		///// <param name="end"></param>
		///// <returns></returns>
		//public List<DAO.DateStatMessage> GetMessagesByDate(DateTime? begin, DateTime? end)
		//{
		//	CheckOpened();
		//	return _dataAdapter.GetMessagesByDate("*", begin, end);
		//}


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

			MessageSettings messageSettings = _appConfig.MessageSettings();
			if (messageSettings.DeleteAfterReceive)
			{
				try
				{
					resMsg.SetStatus(MessageStatus.DELETED, "Удалено после приема.");
					SaveMessage(resMsg);
				}
				catch (Exception ex)
				{
					_logger.LogTrace(ex.ToString());
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


		#region IHostedService  
		public Task StartAsync(CancellationToken cancellationToken)
		{
			Console.Title = $"#{this.ProcessId} ({this.VirtAddress})";
			_status.Created = true;

			return Task.Run(() =>
				{
					_logger.LogTrace("Starting...");

					ChannelSettings channelSettings = _appConfig.ChannelSettings();
					if (channelSettings.AutoOpen)
					{
						_control.OpenChannel();
						if (channelSettings.AutoRun)
							_control.RunChannel();
					}

					_logger.LogTrace("Started");
				}, cancellationToken);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.Run(() =>
				{
					_control.CloseChannel();
					_logger.LogTrace("Shutdown");
				}, cancellationToken);
		}
		#endregion


		#region Helper
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
			if (!_status.Opened)
				throw new InvalidOperationException("Сервис-канал закрыт.");
		}
		#endregion


		//#region IDisposable
		//private bool _disposed = false;

		//protected virtual void Dispose(bool disposing)
		//{
		//	if (_disposed)
		//		return;

		//	if (disposing)
		//	{
		//		// TODO: dispose managed state (managed objects).
		//		try
		//		{
		//			if (_cancellationSource != null && !_cancellationSource.IsCancellationRequested)
		//				_cancellationSource.Cancel(false);
		//		}
		//		catch (Exception ex)
		//		{
		//			if (_logger != null)
		//				_logger.LogError(ex);
		//		}

		//		if (_database != null)
		//			_database.Close();

		//		if (_status != null)
		//		{
		//			_status.Created = false;
		//			_status.Opened = false;
		//			_status.Running = false;
		//			_status.Online = null;
		//		}
		//	}

		//	// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
		//	// TODO: set large fields to null.
		//	_disposed = true;
		//}

		//// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		//// ~ChannelService()
		//// {
		////   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		////   Dispose(false);
		//// }

		//// This code added to correctly implement the disposable pattern.
		//public void Dispose()
		//{
		//	// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//	Dispose(true);
		//	// TODO: uncomment the following line if the finalizer is overridden above.
		//	// GC.SuppressFinalize(this);
		//}
		//#endregion

	}
}
