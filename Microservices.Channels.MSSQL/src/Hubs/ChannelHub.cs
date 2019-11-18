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
		private IChannelService _service;
		private IHostApplicationLifetime _lifetime;


		#region Ctor
		public ChannelHub(IChannelService service, IHostApplicationLifetime lifetime)
		{
			_service = service ?? throw new ArgumentNullException(nameof(service));
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
				_service.Open();
				LogTrace("Opened");
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}

		public void Close()
		{
			try
			{
				LogTrace("Closing");
				_lifetime.StopApplication();
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
				_service.Run();
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
				_service.Stop();
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
			if (_service.TryConnect(out Exception error))
				return null;
			else
				return error;
		}

		public Exception CheckState()
		{
			try
			{
				_service.CheckState();
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
				_service.Repair();
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
				_service.Ping();
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
			return _service.ChannelSettings.GetAppSettings();
		}

		public void SetSettings(IDictionary<string, string> settings)
		{
			_service.ChannelSettings.SetAppSettings(settings);
		}

		//public void SaveSettings()
		//{
		//	_service.Settings.Save();
		//}
		#endregion


		#region Messages
		public List<Message> SelectMessages(QueryParams queryParams)
		{
			return _service.SelectMessages(queryParams);
		}

		public (List<Message>, int) GetMessages(string status, int? skip, int? take)
		{
			return (_service.GetMessages(status, skip, take, out int totalCount), totalCount);
		}

		public (List<Message>, int) GetLastMessages(string status, int? skip, int? take)
		{
			return (_service.GetLastMessages(status, skip, take, out int totalCount), totalCount);
		}

		public Message GetMessage(int msgLink)
		{
			return _service.GetMessage(msgLink);
		}

		public Message FindMessage(int msgLink)
		{
			return _service.FindMessage(msgLink);
		}

		public Message FindMessageByGuid(string msgGuid, string direction)
		{
			return _service.FindMessage(msgGuid, direction);
		}

		public void SaveMessage(Message msg)
		{
			_service.SaveMessage(msg);
		}

		public void DeleteMessage(int msgLink)
		{
			_service.DeleteMessage(msgLink);
		}

		public void DeleteExpiredMessages(DateTime expiredDate, List<string> statuses)
		{
			_service.DeleteExpiredMessages(expiredDate, statuses);
		}

		public void DeleteMessages(IEnumerable<int> msgLinks)
		{
			_service.DeleteMessages(msgLinks);
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
			using (MessageBody body = _service.GetMessageBody(msgLink))
			{
				int bufferSize = _service.ServiceSettings.BufferSize;
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
					using (MessageBody body = _service.GetMessageBody(bodyInfo.MessageLINK))
					{
						body.ApplyInfo(bodyInfo);
						body.Value = new AsyncStreamTextReader(bodyStream);
						_service.SaveMessageBody(body);
					}
				});
		}

		/// <summary>
		/// Удалить тело сообщения.
		/// </summary>
		/// <param name="msgLink"></param>
		public void DeleteMessageBody(int msgLink)
		{
			_service.DeleteMessageBody(msgLink);
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
			using (MessageContent content = _service.GetMessageContent(contentLink))
			{
				int bufferSize = _service.ServiceSettings.BufferSize;
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
						_service.SaveMessageContent(content);
					}
				});
		}

		/// <summary>
		/// Удалить контент сообщения.
		/// </summary>
		/// <param name="contentLink"></param>
		public void DeleteMessageContent(int contentLink)
		{
			_service.DeleteMessageContent(contentLink);
		}
		#endregion


		public int? ReceiveMessage(int msgLink)
		{
			try
			{
				return _service.ReceiveMessage(msgLink);
			}
			catch (Exception ex)
			{
				LogError(ex);
				throw;
			}
		}
		#endregion


		//public override Task OnDisconnectedAsync(Exception exception)
		//{
		//	return base.OnDisconnectedAsync(exception);
		//}

		//public override Task OnConnectedAsync()
		//{
		//	return base.OnConnectedAsync();
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
		void LogError(Exception error)
		{
			_service.LogError(error);
			this.Clients.Caller.ReceiveLog(this.Context.ConnectionId, _service.VirtAddress, "ERROR", error.ToString());
		}

		void LogError(string text, Exception error)
		{
			_service.LogError(text, error);
			this.Clients.Caller.ReceiveLog(this.Context.ConnectionId, _service.VirtAddress, "ERROR", text + Environment.NewLine + error.ToString());
		}

		void LogInfo(string text)
		{
			_service.LogInfo(text);
			this.Clients.Caller.ReceiveLog(this.Context.ConnectionId, _service.VirtAddress, "INFO", text);
		}

		void LogTrace(string text)
		{
			_service.LogTrace(text);
			this.Clients.Caller.ReceiveLog(this.Context.ConnectionId, _service.VirtAddress, "TRACE", text);
		}
		#endregion

	}
}
