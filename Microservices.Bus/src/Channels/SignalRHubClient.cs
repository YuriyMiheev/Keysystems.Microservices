using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microservices.Bus.Logging;
using Microservices.Channels;
using Microservices.Configuration;
using Microservices.Data;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Bus.Channels
{
	public class SignalRHubClient : IChannelClient
	{
		private readonly UriBuilder _hubUrl;
		private readonly ILogger _logger;

		private IDictionary<string, object> _info;
		private HubConnection _hubConnection;
		private IDisposable _logHandler;
		private IDisposable _messagesHandler;
		private IDisposable _statusHandler;
		private ActionBlock<IDictionary<string, object>> _logAction;
		private ActionBlock<Message[]> _messagesAction;
		private ActionBlock<(string, object)> _statusAction;


		#region Ctor
		public SignalRHubClient(string url, ChannelStatus status, ILogger logger)
		{
			_hubUrl = new UriBuilder(url);
			this.Status = status ?? throw new ArgumentNullException(nameof(status));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
		#endregion


		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public ChannelStatus Status { get; }

		public IWebProxy WebProxy { get; set; }

		public bool IsConnected
		{
			get
			{
				if (_hubConnection == null)
					return false;

				return (_hubConnection.State == HubConnectionState.Connected);
			}
		}
		#endregion


		#region Events
		public event Action<IChannelClient> Connected;

		public event Func<IChannelClient, Exception, Task> Reconnecting;

		public event Func<IChannelClient, string, Task> Reconnected;

		public event Func<IChannelClient, Exception, Task> Disconnected;

		public event Action<IChannelClient, IDictionary<string, object>> LogReceived;

		public event Action<IChannelClient, Message[]> MessagesReceived;
		#endregion


		#region Connect/Disconnect
		public async Task ConnectAsync(string accessKey, CancellationToken cancellationToken = default)
		{
			if (this.IsConnected)
				throw new InvalidOperationException($"Подключение к хабу {_hubUrl} уже выполнено.");

			this._info.Clear();

			var uri = new UriBuilder(_hubUrl.Uri);
			uri.Path += "ChannelHub";

			_hubConnection = CreateConnection(uri.Uri);
			await _hubConnection.StartAsync(cancellationToken);

			_info = await _hubConnection.InvokeAsync<IDictionary<string, object>>("Login", accessKey, cancellationToken);
			if (_info == null)
			{
				await _hubConnection.StopAsync();
				throw new InvalidOperationException("Неверный ключ доступа.");
			}

			this.Connected?.Invoke(this);

			_logAction = new ActionBlock<IDictionary<string, object>>(ReceiveLogAction, new ExecutionDataflowBlockOptions() { CancellationToken = cancellationToken });
			_logHandler = _hubConnection.On<IDictionary<string, object>>("ReceiveLog", OnReceiveLog);

			_messagesAction = new ActionBlock<Message[]>(ReceiveMessagesAction, new ExecutionDataflowBlockOptions() { CancellationToken = cancellationToken });
			_messagesHandler = _hubConnection.On<Message[]>("ReceiveMessages", OnReceiveMessages);

			_statusAction = new ActionBlock<(string, object)>(ReceiveStatusAction, new ExecutionDataflowBlockOptions() { CancellationToken = cancellationToken });
			_statusHandler = _hubConnection.On<string, object>("ReceiveStatus", OnReceiveStatus);
		}

		public async Task DisconnectAsync(CancellationToken cancellationToken = default)
		{
			if (this.IsConnected)
				await _hubConnection.StopAsync(cancellationToken);
		}
		#endregion


		#region Control
		public Task OpenChannelAsync(CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("OpenChannel", cancellationToken);
		}

		public Task CloseChannelAsync(CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("CloseChannel", cancellationToken);
		}

		public Task RunChannelAsync(CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("RunChannel", cancellationToken);
		}

		public Task StopChannelAsync(CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("StopChannel", cancellationToken);
		}
		#endregion


		#region Diagnostic
		public Task<Exception> TryConnectToChannelAsync(CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<Exception>("TryConnect", cancellationToken);
		}

		public Task<Exception> CheckChannelStateAsync(CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<Exception>("CheckState", cancellationToken);
		}

		public Task RepairChannelAsync(CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("Repair", cancellationToken);
		}

		public Task PingChannelAsync(CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("Ping", cancellationToken);
		}
		#endregion


		#region Settings
		public Task<IDictionary<string, AppConfigSetting>> GetChannelSettingsAsync(CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<IDictionary<string, AppConfigSetting>>("GetSettings", cancellationToken);
		}

		public Task SetChannelSettingsAsync(IDictionary<string, string> settings, CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("SetSettings", settings, cancellationToken);
		}

		public Task SaveChannelSettings(CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("SaveSettings", cancellationToken);
		}
		#endregion


		#region Messages
		public Task<List<Message>> SelectMessagesAsync(QueryParams queryParams, CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<List<Message>>("SelectMessages", queryParams, cancellationToken);
		}

		public Task<(List<Message>, int)> GetMessagesAsync(string status, int? skip, int? take, CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<(List<Message>, int)>("GetMessages", status, skip, take, cancellationToken);
		}

		public Task<(List<Message>, int)> GetLastMessagesAsync(string status, int? skip, int? take, CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<(List<Message>, int)>("GetLastMessages", status, skip, take, cancellationToken);
		}

		public Task<Message> GetMessageAsync(int msgLink, CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<Message>("GetMessage", msgLink, cancellationToken);
		}

		//Task<Message> IChannelHub_v1.FindMessageAsync(int msgLink, CancellationToken cancellationToken = default)
		//{
		//	CheckConnected();
		//	return _hubConnection.InvokeAsync<Message>("FindMessage", msgLink, cancellationToken);
		//}

		public Task<Message> FindMessageByGuidAsync(string msgGuid, string direction, CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<Message>("FindMessageByGuid", msgGuid, direction, cancellationToken);
		}

		public Task SaveMessageAsync(Message msg, CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("SaveMessage", msg, cancellationToken);
		}

		public Task DeleteMessageAsync(int msgLink, CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("DeleteMessage", msgLink, cancellationToken);
		}

		//Task IChannelHub_v1.DeleteExpiredMessagesAsync(DateTime expiredDate, List<string> statuses, CancellationToken cancellationToken = default)
		//{
		//	CheckConnected();
		//	return _hubConnection.InvokeAsync("DeleteExpiredMessages", expiredDate, statuses, cancellationToken);
		//}

		public Task DeleteMessagesAsync(IEnumerable<int> msgLinks, CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("DeleteMessages", msgLinks, cancellationToken);
		}

		public Task<int?> ReceiveMessageAsync(int msgLink, CancellationToken cancellationToken = default)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<int?>("ReceiveMessage", msgLink, cancellationToken);
		}


		#region Body
		public Task<TextReader> ReadMessageBodyAsync(int msgLink, CancellationToken cancellationToken = default)
		{
			CheckConnected();
			IAsyncEnumerable<char[]> bodyStream = _hubConnection.StreamAsync<char[]>("ReadMessageBody", msgLink, cancellationToken);
			return Task.Run<TextReader>(() => new AsyncStreamTextReader(bodyStream), cancellationToken);
		}

		public Task SaveMessageBodyAsync(MessageBodyInfo bodyInfo, TextReader bodyStream, CancellationToken cancellationToken = default)
		{
			CheckConnected();
			async IAsyncEnumerable<char[]> streamData()
			{
				var buffer = new char[4096 * 1024];
				int charsReaded;
				do
				{
					charsReaded = await bodyStream.ReadAsync(buffer, 0, buffer.Length);
					if (charsReaded > 0)
						yield return buffer.Take(charsReaded).ToArray();
				} while (charsReaded > 0);
			}

			return _hubConnection.InvokeAsync("SaveMessageBody", bodyInfo, streamData(), cancellationToken);
		}
		#endregion

		#region Content
		public Task<TextReader> ReadMessageContentAsync(int contentLink, CancellationToken cancellationToken = default)
		{
			CheckConnected();
			IAsyncEnumerable<char[]> contentStream = _hubConnection.StreamAsync<char[]>("ReadMessageContent", contentLink, cancellationToken);
			return Task.Run<TextReader>(() => new AsyncStreamTextReader(contentStream), cancellationToken);
		}

		public Task SaveMessageContentAsync(MessageContentInfo contentInfo, TextReader contentStream, CancellationToken cancellationToken = default)
		{
			CheckConnected();
			async IAsyncEnumerable<char[]> StreamData()
			{
				var buffer = new char[4096 * 1024];
				int charsReaded;
				do
				{
					charsReaded = await contentStream.ReadAsync(buffer, 0, buffer.Length);
					if (charsReaded > 0)
						yield return buffer.Take(charsReaded).ToArray();
				} while (charsReaded > 0);
			}

			return _hubConnection.InvokeAsync("SaveMessageContent", contentInfo, StreamData(), cancellationToken);
		}
		#endregion

		#endregion


		#region Helpers
		private HubConnection CreateConnection(Uri uri)
		{
			HubConnection hubConnection = new HubConnectionBuilder()
				.WithUrl(uri, o =>
				{
					if (this.WebProxy != null)
						o.Proxy = this.WebProxy;
				})
				.WithAutomaticReconnect()
				.AddMessagePackProtocol()
				.Build();

			hubConnection.Closed += (error) => this.Disconnected?.Invoke(this, error);
			hubConnection.Reconnecting += (error) => this.Reconnecting?.Invoke(this, error);
			hubConnection.Reconnected += (connectionId) =>
				{
					this._info["ConnectionId"] = connectionId;
					return this.Reconnected?.Invoke(this, connectionId);
				};
			return hubConnection;
		}

		private void CheckConnected()
		{
			if (!this.IsConnected)
				throw new InvalidOperationException($"Отсутствует подключение к хабу: {_hubUrl}");
		}

		void OnReceiveMessages(Message[] messages)
		{
			_messagesAction.Post(messages);
		}

		void ReceiveMessagesAction(Message[] messages)
		{
			this.MessagesReceived?.Invoke(this, messages);
		}

		void OnReceiveLog(IDictionary<string, object> logRecord)
		{
			_logAction.Post(logRecord);
		}

		void ReceiveLogAction(IDictionary<string, object> logRecord)
		{
			this.LogReceived?.Invoke(this, logRecord);
		}

		void OnReceiveStatus(string statusName, object statusValue)
		{
			_statusAction.Post((statusName, statusValue));
		}

		void ReceiveStatusAction((string, object) status)
		{
			string statusName = status.Item1;
			object statusValue = status.Item2;

			switch (statusName)
			{
				case nameof(this.Status.Opened):
					this.Status.Opened = (bool)statusValue;
					break;
				case nameof(this.Status.Running):
					this.Status.Running = (bool)statusValue;
					break;
				case nameof(this.Status.Online):
					this.Status.Online = (bool?)statusValue;
					break;
			}
		}
		#endregion


		#region IDisposable
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					if (_logHandler != null)
						_logHandler.Dispose();

					if (_messagesHandler != null)
						_messagesHandler.Dispose();

					if (_statusHandler != null)
						_statusHandler.Dispose();

					_hubConnection?.DisposeAsync();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.
				this.Connected = null;
				this.Reconnecting = null;
				this.Reconnected = null;
				this.Disconnected = null;
				this.LogReceived = null;
				this.MessagesReceived = null;

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		~SignalRHubClient()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			GC.SuppressFinalize(this);
		}
		#endregion

	}
}

//public async void GetSensor1Data(CancellationToken cancellationToken = default(CancellationToken))
//{
//	try
//	{
//		var stream = _hubConnection.StreamAsync<byte>("GetSensor1Data", cancellationToken);
//		await foreach (byte data in stream)
//		{
//			Console.WriteLine($"1. {data}");
//		}
//	}
//	catch (OperationCanceledException ex)
//	{
//		//ex.CancellationToken.ThrowIfCancellationRequested();
//	}
//}
