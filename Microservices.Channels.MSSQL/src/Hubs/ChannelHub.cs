using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

using Microservices.Channels.Hubs;
using Microservices.Channels.Configuration;
using Microservices.Channels.Data;

namespace Microservices.Channels.MSSQL.Hubs
{
	public class ChannelHub : Hub<IChannelHubClient>, IChannelHub
	{
		private IChannelService _channelService;
		private IHostApplicationLifetime _lifetime;


		#region Ctor
		public ChannelHub(IChannelService service, IHostApplicationLifetime lifetime)
		{
			_channelService = service ?? throw new ArgumentNullException(nameof(service));
			_lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
		}
		#endregion


		//[HubMethodName("")]
		public string Login(string accessKey)
		{
			if (String.IsNullOrWhiteSpace(accessKey))
				return this.Context.ConnectionId;
			else
				return null;
		}


		#region Control
		//[Authorize]
		public void Open()
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

		public async Task CloseAsync()
		{
			try
			{
				LogTrace("Closing");
				//_lifetime.StopApplication();
				await ((IHostedService)_channelService).StopAsync(CancellationToken.None);
				LogTrace("Closed");
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}

		public void Run()
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

		public void Stop()
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
			return _channelService.GetAppSettings();
		}

		public void SetSettings(IDictionary<string, string> settings)
		{
			_channelService.SetAppSettings(settings);
		}

		public void SaveSettings()
		{
			_channelService.SaveAppSettings();
		}
		#endregion


		#region Messages
		public List<Message> SelectMessages(QueryParams queryParams)
		{
			return _channelService.SelectMessages(queryParams);
		}

		public (List<Message>, int) GetMessages(string status, int? skip, int? take)
		{
			return (_channelService.GetMessages(status, skip, take, out int totalCount), totalCount);
		}

		public (List<Message>, int) GetLastMessages(string status, int? skip, int? take)
		{
			return (_channelService.GetLastMessages(status, skip, take, out int totalCount), totalCount);
		}

		public Message GetMessage(int msgLink)
		{
			return _channelService.GetMessage(msgLink);
		}

		public Message FindMessage(int msgLink)
		{
			return _channelService.FindMessage(msgLink);
		}

		public Message FindMessageByGuid(string msgGuid, string direction)
		{
			return _channelService.FindMessage(msgGuid, direction);
		}

		public void SaveMessage(Message msg)
		{
			_channelService.SaveMessage(msg);
		}

		public void DeleteMessage(int msgLink)
		{
			_channelService.DeleteMessage(msgLink);
		}

		public void DeleteExpiredMessages(DateTime expiredDate, List<string> statuses)
		{
			_channelService.DeleteExpiredMessages(expiredDate, statuses);
		}

		public void DeleteMessages(IEnumerable<int> msgLinks)
		{
			_channelService.DeleteMessages(msgLinks);
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
			using (MessageBody body = _channelService.GetMessageBody(msgLink))
			{
				int bufferSize = _channelService.ServiceSettings.BufferSize;
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
					using (MessageBody body = _channelService.GetMessageBody(bodyInfo.MessageLINK))
					{
						body.ApplyInfo(bodyInfo);
						body.Value = new AsyncStreamTextReader(bodyStream);
						_channelService.SaveMessageBody(body);
					}
				});
		}

		/// <summary>
		/// Удалить тело сообщения.
		/// </summary>
		/// <param name="msgLink"></param>
		public void DeleteMessageBody(int msgLink)
		{
			_channelService.DeleteMessageBody(msgLink);
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
			using (MessageContent content = _channelService.GetMessageContent(contentLink))
			{
				int bufferSize = _channelService.ServiceSettings.BufferSize;
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
						_channelService.SaveMessageContent(content);
					}
				});
		}

		/// <summary>
		/// Удалить контент сообщения.
		/// </summary>
		/// <param name="contentLink"></param>
		public void DeleteMessageContent(int contentLink)
		{
			_channelService.DeleteMessageContent(contentLink);
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


		//public override Task OnConnectedAsync()
		//{
		//	return base.OnConnectedAsync();
		//}

		//public override Task OnDisconnectedAsync(Exception exception)
		//{
		//	return base.OnDisconnectedAsync(exception);
		//}

		//public async IAsyncEnumerable<byte> GetSensor2Data([EnumeratorCancellation] CancellationToken cancellationToken)
		//{
		//	var r = new Random();
		//	while (!cancellationToken.IsCancellationRequested)
		//	{
		//		await Task.Delay(10, cancellationToken);
		//		yield return (byte)r.Next(255);
		//	}
		//}

		#region Logging
		Task LogError(Exception error)
		{
			_channelService.LogError(error);
			return SendLog("ERROR", error);
		}

		Task LogError(string text, Exception error)
		{
			_channelService.LogError(text, error);
			return SendLog("ERROR", text + Environment.NewLine + error);
		}

		Task LogInfo(string text)
		{
			_channelService.LogInfo(text);
			return SendLog("INFO", text);
		}

		Task LogTrace(string text)
		{
			_channelService.LogTrace(text);
			return SendLog("TRACE", text);
		}
		#endregion


		#region Helper
		private Task SendLog(string logLevel, object text)
		{
			var logRecord = new Dictionary<string, string>();
			logRecord.Add("MachineName", Environment.MachineName);
			logRecord.Add("ProcessId", _channelService.ProcessId);
			logRecord.Add("ConnectionId", this.Context.ConnectionId);
			logRecord.Add("VirtAddress", _channelService.VirtAddress);
			logRecord.Add("LogLevel", logLevel);
			logRecord.Add("Text", text.ToString());

			return this.Clients.Caller.ReceiveLog(logRecord);
		}
		#endregion

	}
}
