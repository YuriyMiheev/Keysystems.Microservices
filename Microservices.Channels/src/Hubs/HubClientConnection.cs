using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace Microservices.Channels.Hubs
{
	/// <summary>
	/// 
	/// </summary>
	[DebuggerDisplay("{this.ConnectionId}")]
	public class HubClientConnection : IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionId"></param>
		/// <param name="client"></param>
		public HubClientConnection(string connectionId, IChannelHubCallback client)
		{
			this.ConnectionId = connectionId ?? throw new ArgumentNullException(nameof(connectionId));
			this.Client = client ?? throw new ArgumentNullException(nameof(client));
		}


		/// <summary>
		/// {Get}
		/// </summary>
		public string ConnectionId { get; private set; }

		/// <summary>
		/// {Get}
		/// </summary>
		public IChannelHubCallback Client { get; private set; }

		///// <summary>
		///// {Get,Set}
		///// </summary>
		//public string UserName { get; set; }

		///// <summary>
		///// {Get,Set}
		///// </summary>
		//public string Password { get; set; }

		///// <summary>
		///// {Get,Set}
		///// </summary>
		//public bool Authorized { get; set; }


		#region IDisposable
		private bool _disposed = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.
				this.Client = null;

				_disposed = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		~HubClientConnection()
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
