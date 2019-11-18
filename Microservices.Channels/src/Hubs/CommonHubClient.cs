using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Keysystems.Microservices.Common
{
	public class CommonHubClient : IHubClient
	{
		private CancellationTokenSource _cancellationSource;
		private HubConnection _hubConnection;
		private IDisposable _onReceiveLog;
		private ActionBlock<object> _receiveLogAction;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		public CommonHubClient(string serviceUrl)
			: this(new Uri(serviceUrl))
		{ }

		public CommonHubClient(Uri serviceUrl)
		{
			this.HubUrl = new UriBuilder(serviceUrl);
			_cancellationSource = new CancellationTokenSource();
			_receiveLogAction = new ActionBlock<object>(new Action<object>(OnReceiveLog), new ExecutionDataflowBlockOptions() { CancellationToken = _cancellationSource.Token });
		}


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public UriBuilder HubUrl { get; private set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public IWebProxy WebProxy { get; set; }

		/// <summary>
		/// {Get}
		/// </summary>
		public virtual bool IsConnected
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
		/// <summary>
		/// 
		/// </summary>
		public event Action<IHubClient> Connected;

		/// <summary>
		/// 
		/// </summary>
		public event Func<IHubClient, Exception, Task> Disconnected;

		/// <summary>
		/// 
		/// </summary>
		public event Action<CommonHubClient, string, string> ServiceLog;
		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <param name="accessKey"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public virtual async Task ConnectAsync(string accessKey, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (this.IsConnected)
				throw new InvalidOperationException($"Подключение к хабу {this.HubUrl} уже выполнено.");

			var uri = new UriBuilder(this.HubUrl.Uri);
			uri.Path += "CommonHub";

			_hubConnection = CreateConnection(uri.Uri);
			await _hubConnection.StartAsync(cancellationToken);
			this.ConnectionId = await _hubConnection.InvokeAsync<string>("Connect", accessKey, cancellationToken);
			if (this.ConnectionId == null)
				throw new InvalidOperationException("Неверный ключ доступа.");

			this.Connected?.Invoke(this);

			_onReceiveLog = _hubConnection.On<string, string>("ReceiveLog", new Action<string, string>(ReceiveLog));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public virtual Task DisconnectAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			_onReceiveLog?.Dispose();
			_onReceiveLog = null;

			if (this.IsConnected)
				return _hubConnection.StopAsync(cancellationToken);
			else
				return Task.CompletedTask;
		}

		public virtual Task<IDictionary<string, object>> GetServiceSettingsAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			CheckConnected();
			return _hubConnection.InvokeAsync<IDictionary<string, object>>("GetSettings", cancellationToken);
		}

		public virtual Task StartServiceAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("StartService", cancellationToken);
		}

		public virtual Task StopServiceAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			CheckConnected();
			return _hubConnection.InvokeAsync("StopService", cancellationToken);
		}


		#region Callbacks
		void ReceiveLog(string logLevel, string text)
		{
			_receiveLogAction.Post(new { LogLevel = logLevel, Text = text });
		}

		void OnReceiveLog(dynamic logArgs)
		{
			this.ServiceLog?.Invoke(this, logArgs.LogLevel, logArgs.Text);
		}
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
					_cancellationSource.Cancel();

					// TODO: dispose managed state (managed objects).
					_onReceiveLog?.Dispose();
					_onReceiveLog = null;

					_hubConnection?.DisposeAsync();
					_hubConnection = null;
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.
				this.ServiceLog = null;
				this.Connected = null;
				this.Disconnected = null;

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		~CommonHubClient()
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
