using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Bus.Addins;
using Microservices.Bus.Data;
using Microservices.Channels;

namespace Microservices.Bus.Channels
{
	public class ProcessChannelContext : IChannelContext, IDisposable
	{
		private readonly ChannelInfo _channelInfo;
		private readonly MicroserviceDescription _description;
		private readonly IChannelFactory _factory;
		private readonly IBusDataAdapter _dataAdapter;
		private readonly IMicroserviceClient _client;
		private Process _process;
		private IChannel _channel;

		public ProcessChannelContext(ChannelInfo channelInfo, IMicroserviceClient client, MicroserviceDescription description, IChannelFactory factory, IBusDataAdapter dataAdapter)
		{
			_channelInfo = channelInfo ?? throw new ArgumentNullException(nameof(channelInfo));
			_description = description ?? throw new ArgumentNullException(nameof(description));
			_factory = factory ?? throw new ArgumentNullException(nameof(factory));
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
			_client = client ?? throw new ArgumentNullException(nameof(client));
		}


		public ChannelInfo Info { get => _channelInfo; }

		public IChannel Channel { get => _channel; }

		public ChannelStatus Status { get => _client.Status; }

		public IMicroserviceClient Client { get => _client; }

		/// <summary>
		/// {Get,Set} Ошибка.
		/// </summary>
		public Exception LastError { get; set; }


		public async Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken = default)
		{
			if (_channel != null)
				return _channel;

			return await Task<IChannel>.Run(() =>
				{
					ChannelInfoProperty prop = _channelInfo.FindProperty("X.ProcessId");
					if (prop != null && Int32.TryParse(prop.Value, out int processId))
						_process = Process.GetProcesses().FindProcessById(processId);

					if (_process == null)
					{

						var startInfo = new ProcessStartInfo()
						{
							FileName = System.IO.Path.Combine(_description.BinPath, _description.Type),
							//UseShellExecute = false,
							CreateNoWindow = true,
							Arguments = $"--Urls {_channelInfo.SID}"
						};
						_process = Process.Start(startInfo);

						if (prop == null)
						{
							prop = new ChannelInfoProperty() { Name = "X.ProcessId" };
							_channelInfo.AddNewProperty(prop);
						}

						prop.Value = _process.Id.ToString();
						_dataAdapter.SaveChannelInfo(_channelInfo);
					}

					_channel = _factory.CreateChannel(_channelInfo, _client);
					_client.Status.Created = true;
					return _channel;
				}, cancellationToken);
		}

		public async Task TerminateChannelAsync(CancellationToken cancellationToken = default)
		{
			if (_channel != null)
			{
				try
				{
					await _channel.CloseAsync(cancellationToken);
				}
				finally
				{
					if (_process != null)
					{
						_process.Kill(true);
						_process.Dispose();
						_process = null;
					}

					_channel = null;
				}
			}
		}


		#region IDisposable
		private bool _disposed = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				// TODO: dispose managed state (managed objects).
				if (_channel != null)
				{
					_channel.Dispose();
					_channel = null;
				}

				if (_process != null)
					_process.Dispose();
			}

			// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
			// TODO: set large fields to null.

			_disposed = true;
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~ChannelContext()
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
