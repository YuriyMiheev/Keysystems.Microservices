using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Data;
using Microservices.Data.DAO;

namespace Microservices.Bus.Channels
{
	public class HubChannel : IChannel, IDisposable
	{
		private readonly ChannelInfo _channelInfo;
		private readonly IChannelHubClient _hub;


		public HubChannel(ChannelInfo channelInfo)
		{
			_channelInfo = channelInfo;
			_hub = new ChannelHubClient(channelInfo.SID);
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
			await _hub.LoginAsync(_channelInfo.PasswordIn, cancellationToken);
			await _hub.OpenChannelAsync(cancellationToken);
		}

		public async Task CloseAsync(CancellationToken cancellationToken = default)
		{
			await _hub.CloseChannelAsync();
			_hub.Dispose();
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
			error = _hub.TryConnectAsync().Result;
			return (error == null);
		}

		public void CheckState()
		{
			_hub.CheckStateAsync().Wait();
		}

		public void Ping()
		{
			_hub.PingAsync().Wait();
		}

		public void Repair()
		{
			_hub.RepairAsync().Wait();
		}


		public void DeleteMessage(int msgLink)
		{
			_hub.DeleteMessageAsync(msgLink).Wait();
		}

		public void DeleteMessageBody(int msgLink)
		{
		}

		public void DeleteMessageContent(int contentLink)
		{
		}

		public void DeleteMessages(IEnumerable<int> msgLinks)
		{
			_hub.DeleteMessagesAsync(msgLinks).Wait();
		}

		public Message FindMessage(string msgGuid, string direction)
		{
			return _hub.FindMessageByGuidAsync(msgGuid, direction).Result;
		}

		public List<Message> GetMessages(string status, int? skip, int? take, out int totalCount)
		{
			(List<Message>, int) result = _hub.GetMessagesAsync(status, skip, take).Result;
			totalCount = result.Item2;
			return result.Item1;
		}

		public List<Message> GetLastMessages(string status, int? skip, int? take, out int totalCount)
		{
			(List<Message>, int) result = _hub.GetLastMessagesAsync(status, skip, take).Result;
			totalCount = result.Item2;
			return result.Item1;
		}

		public Message GetMessage(int msgLink)
		{
			return _hub.GetMessageAsync(msgLink).Result;
		}

		public MessageBody GetMessageBody(int msgLink)
		{
			Message msg = _hub.GetMessageAsync(msgLink).Result;
			return null;
		}

		public MessageContent GetMessageContent(int contentLink)
		{
			//_hub.
			return null;
		}


		public List<DateStatMessage> GetMessagesByDate(DateTime? begin, DateTime? end)
		{
			return null;
		}

		public void SaveMessage(Message msg)
		{
			_hub.SaveMessageAsync(msg).Wait();
		}

		public void SaveMessageBody(MessageBody body)
		{
		}

		public void SaveMessageContent(MessageContent content)
		{
		}

		public List<Message> SelectMessages(QueryParams queryParams)
		{
			return null;
		}


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
