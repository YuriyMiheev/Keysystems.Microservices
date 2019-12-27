using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Microservices;
using Microservices.Channels;
using Microservices.Channels.Configuration;
using Microservices.Channels.Data;
using Microservices.Channels.Hubs;
using Microservices.Channels.Logging;
using Microservices.Configuration;
using Microservices.Data;

using Microsoft.AspNetCore.SignalR;

namespace MSSQL.Microservice.Hubs
{
	public class ChannelHub : Hub<IChannelHubClient>, IChannelHub
	{
		private CancellationTokenSource _cancellationSource;
		private readonly IChannelService _channel;
		private readonly IChannelControl _control;
		private readonly ChannelStatus _status;
		private readonly IMessageScanner _scanner;
		private readonly IAppSettingsConfig _appConfig;
		private readonly IChannelDataAdapter _dataAdapter;
		private readonly IHubConnections _connections;
		private readonly ILogger _logger;


		#region Ctor
		public ChannelHub(IAppSettingsConfig appConfig, IChannelService channel, IChannelControl control, ChannelStatus status, IMessageScanner scanner, IChannelDataAdapter dataAdapter, ILogger logger, IHubConnections connections)
		{
			_appConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
			_channel = channel ?? throw new ArgumentNullException(nameof(channel));
			_control = control ?? throw new ArgumentNullException(nameof(control));
			_status = status ?? throw new ArgumentNullException(nameof(status));
			_scanner = scanner ?? throw new ArgumentNullException(nameof(scanner));
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
			_connections = connections ?? throw new ArgumentNullException(nameof(connections));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
		#endregion


		#region Login/Logout
		//[HubMethodName("")]
		public IDictionary<string, object> Login(string accessKey)
		{
			try
			{
				MainSettings settings = _appConfig.MainSettings();
				if ((accessKey ?? "") == settings.PasswordIn)
				{
					string connectionId = this.Context.ConnectionId;
					if (!_connections.TryGet(connectionId, out IHubConnection connection))
					{
						IChannelHubClient client = this.Clients.Client(connectionId);
						connection = new HubConnection(connectionId, client);
						_connections.Add(connection);

						_scanner.NewMessages += Scanner_NewMessages;
						_status.PropertyChanged += Status_Changed;
					}

					return GetCurrentInfo();
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				LogError(ex);
				return null;
			}
		}
		#endregion


		#region Control
		//[Authorize]
		public async Task OpenChannel()
		{
			try
			{
				LogTrace("Opening");
				await _control.OpenChannelAsync();
				LogTrace("Opened");
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}

		public async Task CloseChannel()
		{
			try
			{
				LogTrace("Closing");
				await _control.CloseChannelAsync();
				LogTrace("Closed");
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}

		public async Task RunChannel()
		{
			try
			{
				LogTrace("Running");
				await _control.RunChannelAsync();
				LogTrace("Runned");
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}

		public async Task StopChannel()
		{
			try
			{
				LogTrace("Stopping");
				await _control.StopChannelAsync();
				LogTrace("Stopped");
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}
		#endregion


		#region Diagnostic
		public async Task<Exception> TryConnect()
		{
			if (await _control.TryConnectAsync(out Exception error))
				return null;
			else
				return error;
		}

		public Exception CheckState()
		{
			try
			{
				_control.CheckState();
				return null;
			}
			catch (Exception ex)
			{
				LogError(ex);
				return ex;
			}
		}

		public void Repair()
		{
			try
			{
				_control.Repair();
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}

		public void Ping()
		{
			try
			{
				_control.Ping();
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}
		#endregion


		#region Settings
		public IDictionary<string, AppConfigSetting> GetSettings()
		{
			return _appConfig.GetAppSettings();
		}

		public void SetSettings(IDictionary<string, string> settings)
		{
			_appConfig.SetAppSettings(settings);
		}

		public void SaveSettings()
		{
			_appConfig.SaveAppSettings();
		}
		#endregion


		#region Messages
		public List<Message> SelectMessages(QueryParams queryParams)
		{
			return _dataAdapter.SelectMessages(queryParams);
		}

		public (List<Message>, int) GetMessages(string status, int? skip, int? take)
		{
			return (_dataAdapter.GetMessages(status, skip, take, out int totalCount), totalCount);
		}

		public (List<Message>, int) GetLastMessages(string status, int? skip, int? take)
		{
			return (_dataAdapter.GetLastMessages(status, skip, take, out int totalCount), totalCount);
		}

		public Message GetMessage(int msgLink)
		{
			return _dataAdapter.GetMessage(msgLink);
		}

		//public Message FindMessage(int msgLink)
		//{
		//	return _dataAdapter.FindMessage(msgLink);
		//}

		public Message FindMessageByGuid(string msgGuid, string direction)
		{
			return _dataAdapter.FindMessage(msgGuid, direction);
		}

		public void SaveMessage(Message msg)
		{
			_dataAdapter.SaveMessage(msg);
		}

		public void DeleteMessage(int msgLink)
		{
			_dataAdapter.DeleteMessage(msgLink);
		}

		//public void DeleteExpiredMessages(DateTime expiredDate, List<string> statuses)
		//{
		//	_dataAdapter.DeleteExpiredMessages(expiredDate, statuses);
		//}

		public void DeleteMessages(IEnumerable<int> msgLinks)
		{
			_dataAdapter.DeleteMessages(msgLinks);
		}


		#region Body
		/// <summary>
		/// Получить тело сообщения.
		/// </summary>
		/// <param name="msgLink"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async IAsyncEnumerable<char[]> ReadMessageBody(int msgLink, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			using (MessageBody body = _dataAdapter.GetMessageBody(msgLink))
			{
				int bufferSize = _appConfig.XSettings().BufferSize;
				var buffer = new char[bufferSize];
				int charsReaded;
				do
				{
					charsReaded = await body.Value.ReadAsync(buffer, 0, buffer.Length);
					if (charsReaded > 0 && !cancellationToken.IsCancellationRequested)
						yield return buffer.Take(charsReaded).ToArray();
				} while (charsReaded > 0 && !cancellationToken.IsCancellationRequested);
			}
		}

		/// <summary>
		/// Сохранить тело сообщения.
		/// </summary>
		/// <param name="bodyInfo"></param>
		/// <param name="bodyStream"></param>
		public Task SaveMessageBody(MessageBodyInfo bodyInfo, IAsyncEnumerable<char[]> bodyStream, CancellationToken cancellationToken = default)
		{
			return Task.Run(() =>
				{
					using (MessageBody body = _dataAdapter.GetMessageBody(bodyInfo.MessageLINK))
					{
						body.ApplyInfo(bodyInfo);
						body.Value = new AsyncStreamTextReader(bodyStream);
						_dataAdapter.SaveMessageBody(body);
					}
				});
		}

		/// <summary>
		/// Удалить тело сообщения.
		/// </summary>
		/// <param name="msgLink"></param>
		public void DeleteMessageBody(int msgLink)
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
		public async IAsyncEnumerable<char[]> ReadMessageContent(int contentLink, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			using (MessageContent content = _dataAdapter.GetMessageContent(contentLink))
			{
				int bufferSize = _appConfig.XSettings().BufferSize;
				var buffer = new char[bufferSize];
				int charsReaded;
				do
				{
					charsReaded = await content.Value.ReadAsync(buffer, 0, buffer.Length);
					if (charsReaded > 0 && !cancellationToken.IsCancellationRequested)
						yield return buffer.Take(charsReaded).ToArray();
				} while (charsReaded > 0 && !cancellationToken.IsCancellationRequested);
			}
		}

		/// <summary>
		/// Сохранить контент сообщения.
		/// </summary>
		/// <param name="contentInfo"></param>
		/// <param name="stream"></param>
		/// <param name="cancellationToken"></param>
		public Task SaveMessageContent(MessageContentInfo contentInfo, IAsyncEnumerable<char[]> stream, CancellationToken cancellationToken = default)
		{
			return Task.Run(() =>
				{
					using (var content = new MessageContent())
					{
						content.ApplyInfo(contentInfo);
						content.Value = new AsyncStreamTextReader(stream);
						_dataAdapter.SaveMessageContent(content);
					}
				});
		}

		/// <summary>
		/// Удалить контент сообщения.
		/// </summary>
		/// <param name="contentLink"></param>
		public void DeleteMessageContent(int contentLink)
		{
			_dataAdapter.DeleteMessageContent(contentLink);
		}
		#endregion


		public int? ReceiveMessage(int msgLink)
		{
			try
			{
				return _channel.ReceiveMessage(msgLink);
			}
			catch (Exception ex)
			{
				LogError(ex);
				throw;
			}
		}

		public void SendMessage(int msgLink)
		{
			try
			{
				_channel.SendMessage(msgLink);
			}
			catch (Exception ex)
			{
				LogError(ex);
				throw;
			}
		}
		#endregion


		#region Override
		//public override Task OnConnectedAsync()
		//{
		//	return base.OnConnectedAsync();
		//}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			try
			{
				if (_connections.TryRemove(this.Context.ConnectionId, out IHubConnection connection))
					connection.Dispose();

				return base.OnDisconnectedAsync(exception);
			}
			catch (Exception ex)
			{
				LogError(ex);
				throw;
			}
		}
		#endregion


		#region Logging
		void LogError(Exception error)
		{
			_logger.LogError(error);
			SendLog("ERROR", error.ToString());
		}

		void LogError(string text, Exception error)
		{
			_logger.LogError(text, error);
			SendLog("ERROR", text + Environment.NewLine + error);
		}

		void LogInfo(string text)
		{
			_logger.LogInfo(text);
			SendLog("INFO", text);
		}

		void LogTrace(string text)
		{
			_logger.LogTrace(text);
			SendLog("TRACE", text);
		}
		#endregion


		#region Helper
		private bool SendLog(string logLevel, string text)
		{
			IDictionary<string, object> record = GetCurrentInfo();
			record.Add("LogLevel", logLevel);
			record.Add("Text", text);

			return _connections.SendLogToClient(record);
		}

		private IDictionary<string, object> GetCurrentInfo()
		{
			var result = new Dictionary<string, object>();
			result.Add("MachineName", Environment.MachineName);
			result.Add("ProcessId", _channel.ProcessId);
			result.Add("ConnectionId", this.Context.ConnectionId);
			result.Add("VirtAddress", _channel.VirtAddress);
			return result;
		}

		private bool Scanner_NewMessages(Message[] messages)
		{
			_logger.LogTrace($"SendMessages: {messages.Length}");
			return _connections.SendMessagesToClient(messages);
		}

		private void Status_Changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var statuses = new Dictionary<string, object>
			{
				{ nameof(_status.Created), _status.Created },
				{ nameof(_status.Opened), _status.Opened },
				{ nameof(_status.Running), _status.Running },
				{ nameof(_status.Online), _status.Online }
			};

			//_logger.LogTrace($"SendStatus: {statusName}={statusValue}");
			_connections.SendStatusToClient(statuses);
		}
		#endregion

	}
}

//public async IAsyncEnumerable<byte> GetSensor2Data([EnumeratorCancellation] CancellationToken cancellationToken)
//{
//	var r = new Random();
//	while (!cancellationToken.IsCancellationRequested)
//	{
//		await Task.Delay(10, cancellationToken);
//		yield return (byte)r.Next(255);
//	}
//}
