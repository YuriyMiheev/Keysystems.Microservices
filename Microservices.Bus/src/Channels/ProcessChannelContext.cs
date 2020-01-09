using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Bus.Addins;
using Microservices.Bus.Data;
using Microservices.Channels;
using Microservices.Logging;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Контекст канала, работающего в отдельном процессе.
	/// </summary>
	public class ProcessChannelContext : IChannelContext, IDisposable
	{
		private readonly IAddinDescription _description;
		private readonly Func<ChannelInfo, IChannelClient, ILogger, IChannel> _createChannel;
		private readonly IBusDataAdapter _dataAdapter;
		private readonly ILogger _logger;
		private Process _process;


		public ProcessChannelContext(IAddinDescription description, ChannelInfo channelInfo, IChannelClient channelClient, IBusDataAdapter dataAdapter, ILogger logger, Func<ChannelInfo, IChannelClient, ILogger, IChannel> createChannel)
		{
			_description = description ?? throw new ArgumentNullException(nameof(description));
			this.Info = channelInfo ?? throw new ArgumentNullException(nameof(channelInfo));
			this.Client = channelClient ?? throw new ArgumentNullException(nameof(channelClient));
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_createChannel = createChannel ?? throw new ArgumentNullException(nameof(createChannel));
		}


		public ChannelInfo Info { get; }

		public IChannel Channel { get; private set; }

		public ChannelStatus Status { get => this.Client.Status; }

		public IChannelClient Client { get; }

		public Exception LastError { get; set; }


		public async Task<IChannel> ActivateChannelAsync(CancellationToken cancellationToken = default)
		{
			//await Task.Run(async () => 
			//	{
			ChannelInfoProperty prop = this.Info.FindProperty("X.ProcessId");
			if (prop != null && Int32.TryParse(prop.Value, out int processId))
				_process = Process.GetProcesses().FindProcessById(processId);

			if (_process == null)
			{
				bool showWindow = this.Info.XSettings().ShowWindow;
				var startInfo = new ProcessStartInfo()
					{
						FileName = System.IO.Path.Combine(_description.AddinPath, _description.Type),
						UseShellExecute = showWindow,
						CreateNoWindow = !showWindow,
						WindowStyle = (showWindow ? ProcessWindowStyle.Minimized : ProcessWindowStyle.Normal),
						Arguments = $"--Urls {this.Info.SID}",
						WorkingDirectory = _description.AddinPath
					};
				_process = Process.Start(startInfo);
				_process.EnableRaisingEvents = true;
				_process.Exited += Process_Exited;

				if (prop == null)
				{
					prop = new ChannelInfoProperty() { Name = "X.ProcessId" };
					this.Info.AddNewProperty(prop);
				}

				prop.Value = _process.Id.ToString();
				_dataAdapter.SaveChannelInfo(this.Info);
			}

			this.Channel = _createChannel(this.Info, this.Client, _logger);
			await this.Client.ConnectAsync(this.Info.PasswordIn, cancellationToken);


			string title = $"#{_process.Id} | #{this.Info.LINK} ({this.Info.VirtAddress})";
			await this.Client.SetWindowTitleAsync(title, cancellationToken);

			this.Status.Created = true;
			return this.Channel;
			//}, cancellationToken);
		}

		private void Process_Exited(object sender, EventArgs e)
		{
			_logger.LogTrace($"Процесс #{_process.Id} канала {this.Info.Id} завершил работу.");

			try
			{
				if (this.Channel != null)
					this.Channel.Dispose();
			}
			finally
			{
				this.Status.Online = null;
				this.Status.Running = false;
				this.Status.Opened = false;
				this.Status.Created = false;

				_process.Dispose();
			}
		}

		public async Task TerminateChannelAsync(CancellationToken cancellationToken = default)
		{
			_logger.LogTrace($"Прерывание работы канала {this.Info.Id}.");

			try
			{
				if (this.Channel != null && this.Status.Created)
					await this.Channel.CloseAsync(cancellationToken);
			}
			finally
			{
				if (this.Channel != null)
					this.Channel.Dispose();

				this.Status.Created = false;

				if (_process != null)
				{
					_process.Kill(true);
					_process.Dispose();
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
				if (this.Channel != null)
					this.Channel.Dispose();

				this.Status.Created = false;

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
