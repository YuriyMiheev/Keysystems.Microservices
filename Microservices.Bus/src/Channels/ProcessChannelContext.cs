using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Bus.Channels
{
	public class ProcessChannelContext : IChannelContext, IDisposable
	{
		private readonly ChannelInfo _channelInfo;
		private readonly IChannelFactory _factory;
		private Process _process;
		private IChannel _channel;


		public ProcessChannelContext(ChannelInfo channelInfo, IChannelFactory factory)
		{
			_channelInfo = channelInfo ?? throw new ArgumentNullException(nameof(channelInfo));
			_factory = factory ?? throw new ArgumentNullException(nameof(factory));
		}


		public ChannelInfo ChannelInfo { get => _channelInfo; }

		public bool IsChannelCreated
		{
			get { return (_channel != null); }
		}

		public IChannel Channel { get => _channel; }

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
					var startInfo = new ProcessStartInfo()
					{
						FileName = _channelInfo.Description.BinPath,
						//UseShellExecute = false,
						CreateNoWindow = true,
						Arguments = $"--Urls {_channelInfo.SID}"
					};
					_process = Process.Start(startInfo);

					return _channel = _factory.CreateChannel(_channelInfo);
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
						_process.Kill(true);

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
