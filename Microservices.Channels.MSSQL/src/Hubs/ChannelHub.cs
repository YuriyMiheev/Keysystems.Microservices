﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Channels.Configuration;
using Microservices.Channels.Data;
using Microservices.Channels.Hubs;
using Microservices.Channels.Logging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace Microservices.Channels.MSSQL.Hubs
{
	public class ChannelHub : Hub<IChannelHubClient>, IChannelHub
	{
		private IChannelService _channelService;
		private IAppSettingsConfiguration _appConfig;
		private IMessageDataAdapter _dataAdapter;
		private IHubClientConnections _connections;
		private ServiceSettings _serviceSettings;
		private ILogger _logger;


		#region Ctor
		public ChannelHub(IChannelService channelService, IAppSettingsConfiguration appConfig, IMessageDataAdapter dataAdapter, ILogger logger, IHubClientConnections connections)
		{
			_channelService = channelService ?? throw new ArgumentNullException(nameof(channelService));
			_appConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_connections = connections ?? throw new ArgumentNullException(nameof(connections));

			_serviceSettings = _appConfig.ServiceSettings();
		}
		#endregion


		//[HubMethodName("")]
		public string Login(string accessKey)
		{
			if (String.IsNullOrWhiteSpace(accessKey))
			{
				string connectionId = this.Context.ConnectionId;
				if (!_connections.TryGet(connectionId, out HubClientConnection connection))
				{
					IChannelHubClient client = this.Clients.Client(connectionId);
					connection = new HubClientConnection(connectionId, client);
					_connections.Add(connection);

					_channelService.OutMessages += SendMessages;
				}

				return this.Context.ConnectionId;
			}
			else
			{
				return null;
			}
		}


		#region Control
		//[Authorize]
		public void OpenChannel()
		{
			try
			{
				LogTrace("Opening");
				_channelService.Open();
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
				await (_channelService as IHostedService)?.StopAsync(CancellationToken.None);
				LogTrace("Closed");
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}

		public void RunChannel()
		{
			try
			{
				LogTrace("Running");
				_channelService.Run();
				LogTrace("Runned");
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}

		public void StopChannel()
		{
			try
			{
				LogTrace("Stopping");
				_channelService.Stop();
				LogTrace("Stopped");
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}
		#endregion


		#region Diagnostic
		public Exception TryConnect()
		{
			if (_channelService.TryConnect(out Exception error))
				return null;
			else
				return error;
		}

		public Exception CheckState()
		{
			try
			{
				_channelService.CheckState();
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
				_channelService.Repair();
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
				_channelService.Ping();
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}
		#endregion


		#region Settings
		public IDictionary<string, ConfigFileSetting> GetSettings()
		{
			return _appConfig.GetAppSettings();
		}

		public void SetSettings(IDictionary<string, string> settings)
		{
			//if (_channelService.Opened)
			//	throw new InvalidOperationException("Операция недопустима для открытого сервис-канала.");

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
				int bufferSize = _serviceSettings.BufferSize;
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
				int bufferSize = _serviceSettings.BufferSize;
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
				return _channelService.ReceiveMessage(msgLink);
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
				_channelService.SendMessage(msgLink);
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
				if (_connections.TryRemove(this.Context.ConnectionId, out HubClientConnection connection))
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
			SendLog("ERROR", error);
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
		private bool SendLog(string logLevel, object text)
		{
			var record = new Dictionary<string, string>();
			record.Add("MachineName", Environment.MachineName);
			record.Add("ProcessId", _channelService.ProcessId);
			record.Add("ConnectionId", this.Context.ConnectionId);
			record.Add("VirtAddress", _channelService.VirtAddress);
			record.Add("LogLevel", logLevel);
			record.Add("Text", text.ToString());

			return _connections.SendLogToClient(record);
		}

		private bool SendMessages(Message[] messages)
		{
			return _connections.SendMessagesToClient(messages);
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
