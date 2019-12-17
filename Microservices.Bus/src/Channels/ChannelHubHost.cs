using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Data;
using Microservices.Data.DAO;

using Hub = Microservices.ChannelConnector;

namespace Microservices.Bus.Channels
{
	public class ChannelHubHost : IChannel, IDisposable
	{
		private readonly ChannelInfo _channelInfo;
		private Hub.IChannelHubClient _hub;


		public ChannelHubHost(ChannelInfo channelInfo)
		{
			_channelInfo = channelInfo ?? throw new ArgumentNullException(nameof(channelInfo));
		}


		public bool IsOpened
		{
			get
			{
				if (_hub == null)
					return false;

				return _hub.IsConnected;
			}
		}


		public async Task OpenAsync(CancellationToken cancellationToken = default)
		{
			_hub = new Hub.ChannelHubClient(_channelInfo.SID);
			await _hub.LoginAsync(_channelInfo.PasswordIn);
			//_hub.
		}

		public async Task CloseAsync(CancellationToken cancellationToken = default)
		{
			await _hub.CloseChannelAsync();
			_hub.Dispose();
			_hub = null;
		}

		public async Task RunAsync(CancellationToken cancellationToken = default)
		{
			await _hub.RunChannelAsync();
		}

		public async Task StopAsync(CancellationToken cancellationToken = default)
		{
			await _hub.StopChannelAsync();
		}


		public bool TryConnect(out Exception error)
		{
			throw new NotImplementedException();
		}

		public void CheckState()
		{
		}

		public void Ping()
		{
		}

		public void Repair()
		{
		}


		public void DeleteMessage(int msgLink)
		{
		}

		public void DeleteMessageBody(int msgLink)
		{
		}

		public void DeleteMessageContent(int contentLink)
		{
		}

		public void DeleteMessages(IEnumerable<int> msgLinks)
		{
		}

		public Message FindMessage(string msgGuid, string direction)
		{
			return _hub.FindMessageByGuidAsync(msgGuid, direction).Result.ToObj();
		}

		public List<Message> GetLastMessages(string status, int? skip, int? take, out int totalCount)
		{
		}

		public Message GetMessage(int msgLink)
		{
		}

		public MessageBody GetMessageBody(int msgLink)
		{
		}

		public MessageContent GetMessageContent(int contentLink)
		{
		}

		public List<Message> GetMessages(string status, int? skip, int? take, out int totalCount)
		{
		}

		public List<DateStatMessage> GetMessagesByDate(DateTime? begin, DateTime? end)
		{
		}

		public void SaveMessage(Message msg)
		{
		}

		public void SaveMessageBody(MessageBody body)
		{
		}

		public void SaveMessageContent(MessageContent content)
		{
		}

		public List<Message> SelectMessages(QueryParams queryParams)
		{
		}


		#region IDisposable Support
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

				_disposed = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~ChannelHost()
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
