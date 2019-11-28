﻿using System;
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


		#region Ctor
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
		#endregion


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


		#region Events
		public event Action<IChannelHubClient> Connected;

		public event Func<IChannelHubClient, Exception, Task> Disconnected;
		#endregion


		#region IChannelHub_v1

		#region Events/Callbacks
		private IDisposable receiveLogHandler_v1;
		private ActionBlock<IDictionary<string, string>> receiveLogAction_v1;
		private object _serviceLogReceivedLock = new Object();
		private event Action<IChannelHubClient, IDictionary<string, string>> serviceLogReceived_v1;
		event Action<IChannelHubClient, IDictionary<string, string>> IChannelHub_v1.LogReceived
		{
			add
			{
				lock (_serviceLogReceivedLock)
				{
					serviceLogReceived_v1 += value;
				}
			}
			remove
			{
				lock (_serviceLogReceivedLock)
				{
					serviceLogReceived_v1 -= value;
				}
			}
		}

		void OnReceiveLog_v1(IDictionary<string, string> logRecord)
		{
			receiveLogAction_v1.Post(logRecord);
		}

		void ReceiveLogAction_v1(IDictionary<string, string> logRecord)
		{
			serviceLogReceived_v1?.Invoke(this, logRecord);
		}


		private IDisposable receiveMessagesHandler_v1;
		private ActionBlock<Message[]> receiveMessagesAction_v1;
		private object _sendMessagesReceivedLock = new Object();
		private event Action<IChannelHubClient, Message[]> sendMessagesReceived_v1;
		event Action<IChannelHubClient, Message[]> IChannelHub_v1.SendMessagesReceived
		{
			add
			{
				lock (_sendMessagesReceivedLock)
				{
					sendMessagesReceived_v1 += value;
				}
			}
			remove
			{
				lock (_sendMessagesReceivedLock)
				{
					sendMessagesReceived_v1 -= value;
				}
			}
		}

		void OnReceiveMessages_v1(Message[] messages)
		{
			receiveMessagesAction_v1.Post(messages);
		}

		void ReceiveMessagesAction_v1(Message[] messages)
		{
			sendMessagesReceived_v1?.Invoke(this, messages);
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

			receiveLogAction_v1 = new ActionBlock<IDictionary<string, string>>(ReceiveLogAction_v1, new ExecutionDataflowBlockOptions() { });
			receiveLogHandler_v1 = _hubConnection.On<IDictionary<string, string>>("Log", OnReceiveLog_v1);

			receiveMessagesAction_v1 = new ActionBlock<Message[]>(ReceiveMessagesAction_v1, new ExecutionDataflowBlockOptions() { });
			receiveMessagesHandler_v1 = _hubConnection.On<Message[]>("OutMessages", OnReceiveMessages_v1);
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
		Task IChannelHub_v1.OpenChannelAsync(CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("OpenChannel", cancellationToken);
		}

		Task IChannelHub_v1.CloseChannelAsync(CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("CloseChannel", cancellationToken);
		}

		Task IChannelHub_v1.RunChannelAsync(CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("RunChannel", cancellationToken);
		}

		Task IChannelHub_v1.StopChannelAsync(CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("StopChannel", cancellationToken);
		}
		#endregion


		#region Diagnostic
		Task<Exception> IChannelHub_v1.TryConnectAsync(CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<Exception>("TryConnect", cancellationToken);
		}

		Task<Exception> IChannelHub_v1.CheckStateAsync(CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<Exception>("CheckState", cancellationToken);
		}

		Task IChannelHub_v1.RepairAsync(CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("Repair", cancellationToken);
		}

		Task IChannelHub_v1.PingAsync(CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("Ping", cancellationToken);
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

		Task IChannelHub_v1.SaveSettings(CancellationToken cancellationToken)
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<IDictionary<string, SettingItem>>("SaveSettings", cancellationToken);
		}
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
					if (receiveLogHandler_v1 != null)
						receiveLogHandler_v1.Dispose();

					if(receiveMessagesHandler_v1 != null)
						receiveMessagesHandler_v1.Dispose();

					_hubConnection?.DisposeAsync();
					_hubConnection = null;
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.
				serviceLogReceived_v1 = null;
				sendMessagesReceived_v1 = null;
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
