using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

//using Keysystems.RemoteMessaging.Adapters;

namespace Microservices.Channels.MSSQL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class SendMessageProcessor : IDisposable
	{
		private IChannelService channel;
		private SendMessageScanner scanner;
		private MessageSender sender;
		private bool started;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="recipient"></param>
		public SendMessageProcessor(IChannelService channel, string recipient)
		{
			this.channel = channel ?? throw new ArgumentNullException("channel");

			this.scanner = new SendMessageScanner(channel, recipient);
			this.sender = new MessageSender(channel);
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public Task Completion
		{
			get { return this.sender.Completion; }
		}
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parallelTasks"></param>
		/// <param name="cancellationToken"></param>
		public void Start(int parallelTasks, CancellationToken cancellationToken = default)
		{
			#region Validate parameters
			if ( parallelTasks <= 0 )
				throw new ArgumentOutOfRangeException("parallelTasks", "Значение должно быть > 0.");

			if ( cancelToken == null )
				throw new ArgumentNullException("cancelToken");
			#endregion

			if ( this.started )
				return;

			lock ( this )
			{
				if ( this.started )
					return;

				this.started = true;
				cancellationToken.Register(() =>
					{
						this.started = false;
					});

				this.sender.StartSendBufferMessages(parallelTasks, cancellationToken);
				this.scanner.Start(this.channel.MessageSettings.ScanInterval, this.sender, cancellationToken);
			}
		}
		#endregion


		#region IDisposable
		private bool disposed = false;

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if ( disposed )
				return;

			if ( disposing )
			{
				this.sender.Dispose();
				this.scanner.Dispose();

				this.channel = null;
			}

			disposed = true;
		}

		/// <summary>
		/// Деструктор.
		/// </summary>
		~SendMessageProcessor()
		{
			Dispose(false);
		}
		#endregion

	}
}
