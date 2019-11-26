using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Channels.Client
{
	public class ChannelHubClient : IChannelHubClient, IChannelHub_v1
	{
		private HubConnection _hubConnection;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		public ChannelHubClient(string serviceUrl)
			: this(new Uri(serviceUrl))
		{ }

		public ChannelHubClient(Uri serviceUrl)
		{
			this.HubUrl = new UriBuilder(serviceUrl);
		}


		#region Properties
		public UriBuilder HubUrl { get; private set; }

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

		/// <summary>
		/// {Get}
		/// </summary>
		public string ConnectionId { get; private set; }
		#endregion


		#region IChannelHub_v1
		private IDisposable _receiveLogHandler_v1;
		private ActionBlock<IDictionary<string, string>> _onReceiveLogAction_v1;
		private Action<IChannelHubClient, IDictionary<string, string>> _serviceLogEventHandler_v1;


		#region Events
		public event Action<IChannelHubClient> Connected;

		public event Func<IChannelHubClient, Exception, Task> Disconnected;
		#endregion


		#region Callbacks
		void IChannelHub_v1.ServiceLogEventHandler(Action<IChannelHubClient, IDictionary<string, string>> eventHandler)
		{
			_serviceLogEventHandler_v1 = eventHandler;
		}

		void ReceiveLog(IDictionary<string, string> logRecord)
		{
			_onReceiveLogAction_v1.Post(logRecord);
		}

		void OnReceiveLog(IDictionary<string, string> logRecord)
		{
			_serviceLogEventHandler_v1?.Invoke(this, logRecord);
		}
		#endregion


		#region Login/Logout
		async Task IChannelHub_v1.LoginAsync(string accessKey, CancellationToken cancellationToken)
		{
			if (this.IsConnected)
				throw new InvalidOperationException($"Подключение к хабу {this.HubUrl} уже выполнено.");

			var uri = new UriBuilder(this.HubUrl.Uri);
			uri.Path += "ChannelHub";

			_hubConnection = CreateConnection(uri.Uri);
			await _hubConnection.StartAsync(cancellationToken);

			this.ConnectionId = await _hubConnection.InvokeAsync<string>("Login", accessKey, cancellationToken);
			if (this.ConnectionId == null)
			{
				await _hubConnection.StopAsync();
				throw new InvalidOperationException("Неверный ключ доступа.");
			}

			this.Connected?.Invoke(this);

			_onReceiveLogAction_v1 = new ActionBlock<IDictionary<string, string>>(new Action<IDictionary<string, string>>(OnReceiveLog), new ExecutionDataflowBlockOptions() { });
			_receiveLogHandler_v1 = _hubConnection.On<IDictionary<string, string>>("ReceiveLog", new Action<IDictionary<string, string>>(ReceiveLog));
		}

		Task IChannelHub_v1.LogoutAsync(CancellationToken cancellationToken)
		{
			if (this.IsConnected)
				return _hubConnection.StopAsync(cancellationToken);
			else
				return Task.CompletedTask;
		}
		#endregion


		#region Control
		Task IChannelHub_v1.OpenAsync(CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("Open", cancellationToken);
		}

		Task IChannelHub_v1.CloseAsync(CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("CloseAsync", cancellationToken);
		}

		Task IChannelHub_v1.RunAsync(CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("Run", cancellationToken);
		}

		Task IChannelHub_v1.StopAsync(CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("Stop", cancellationToken);
		}
		#endregion


		#region Settings
		Task<IDictionary<string, SettingItem>> IChannelHub_v1.GetSettingsAsync(CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<IDictionary<string, SettingItem>>("GetSettings", cancellationToken);
		}

		Task IChannelHub_v1.SetSettingsAsync(IDictionary<string, string> settings, CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<IDictionary<string, SettingItem>>("SetSettings", cancellationToken);
		}

		//Task<IDictionary<string, string>> IChannelHub_v1.GetConnectionStrings(CancellationToken cancellationToken)
		//{
		//	CheckConnected();
		//	return _hubConnection.InvokeAsync<IDictionary<string, string>>("GetConnectionStrings", cancellationToken);
		//}

		//Task IChannelHub_v1.SetConnectionStrings(IDictionary<string, string> connStrings, CancellationToken cancellationToken)
		//{
		//	CheckConnected();
		//	return _hubConnection.InvokeAsync<IDictionary<string, SettingItem>>("SetConnectionStrings", cancellationToken);
		//}

		//public void SaveSettings()
		//{
		//	_service.Settings.Save();
		//}
		#endregion


		#region Messages
		Task<List<Message>> IChannelHub_v1.SelectMessagesAsync(QueryParams queryParams, CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<List<Message>>("SelectMessages", queryParams, cancellationToken);
		}

		Task<(List<Message>, int)> IChannelHub_v1.GetMessagesAsync(string status, int? skip, int? take, CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<(List<Message>, int)>("GetMessages", status, skip, take, cancellationToken);
		}

		Task<(List<Message>, int)> IChannelHub_v1.GetLastMessagesAsync(string status, int? skip, int? take, CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<(List<Message>, int)>("GetLastMessages", status, skip, take, cancellationToken);
		}

		Task<Message> IChannelHub_v1.GetMessageAsync(int msgLink, CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<Message>("GetMessage", msgLink, cancellationToken);
		}

		Task<Message> IChannelHub_v1.FindMessageAsync(int msgLink, CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<Message>("FindMessage", msgLink, cancellationToken);
		}

		Task<Message> IChannelHub_v1.FindMessageByGuidAsync(string msgGuid, string direction, CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<Message>("FindMessageByGuid", msgGuid, direction, cancellationToken);
		}

		Task IChannelHub_v1.SaveMessageAsync(Message msg, CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("SaveMessage", msg, cancellationToken);
		}

		Task IChannelHub_v1.DeleteMessageAsync(int msgLink, CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("DeleteMessage", msgLink, cancellationToken);
		}

		Task IChannelHub_v1.DeleteExpiredMessagesAsync(DateTime expiredDate, List<string> statuses, CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("DeleteExpiredMessages", expiredDate, statuses, cancellationToken);
		}

		Task IChannelHub_v1.DeleteMessagesAsync(IEnumerable<int> msgLinks, CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("DeleteMessages", msgLinks, cancellationToken);
		}

		Task<int?> IChannelHub_v1.ReceiveMessageAsync(int msgLink, CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<int?>("ReceiveMessage", msgLink, cancellationToken);
		}


		#region Body
		Task<TextReader> IChannelHub_v1.ReadMessageBodyAsync(int msgLink, CancellationToken cancellationToken)
		{
			CheckConnected();
			IAsyncEnumerable<char[]> bodyStream = _hubConnection.StreamAsync<char[]>("ReadMessageBody", msgLink, cancellationToken);
			return Task.Run<TextReader>(() => new AsyncStreamTextReader(bodyStream), cancellationToken);
		}

		Task IChannelHub_v1.SaveMessageBodyAsync(MessageBodyInfo bodyInfo, TextReader bodyStream, CancellationToken cancellationToken)
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
		Task<TextReader> IChannelHub_v1.ReadMessageContentAsync(int contentLink, CancellationToken cancellationToken)
		{
			CheckConnected();
			IAsyncEnumerable<char[]> contentStream = _hubConnection.StreamAsync<char[]>("ReadMessageContent", contentLink, cancellationToken);
			return Task.Run<TextReader>(() => new AsyncStreamTextReader(contentStream), cancellationToken);
		}

		Task IChannelHub_v1.SaveMessageContentAsync(MessageContentInfo contentInfo, TextReader contentStream, CancellationToken cancellationToken)
		{
			CheckConnected();
			async IAsyncEnumerable<char[]> streamData()
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

			return _hubConnection.InvokeAsync("SaveMessageContent", contentInfo, streamData(), cancellationToken);
		}
		#endregion

		#endregion

		#endregion

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


		#region Helpers
		private HubConnection CreateConnection(Uri uri)
		{
			HubConnection hubConnection = new HubConnectionBuilder()
				.WithUrl(uri, o =>
				{
					if (this.WebProxy != null)
						o.Proxy = this.WebProxy;
				})
				.AddMessagePackProtocol()
				.Build();

			hubConnection.Closed += (e) => this.Disconnected?.Invoke(this, e);
			return hubConnection;
		}

		private void CheckConnected()
		{
			if (!this.IsConnected)
				throw new InvalidOperationException($"Отсутствует подключение к хабу: {this.HubUrl}");
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
					//_cancellationSource.Cancel();

					_hubConnection?.DisposeAsync();
					_hubConnection = null;
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.
				_serviceLogEventHandler_v1 = null;
				this.Connected = null;
				this.Disconnected = null;

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		~ChannelHubClient()
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
