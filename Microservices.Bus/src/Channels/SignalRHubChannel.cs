using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Data;
using Microservices.Data.DAO;

namespace Microservices.Bus.Channels
{
	public class SignalRHubChannel : IChannel, IDisposable
	{
		private readonly ChannelInfo _channelInfo;
		private readonly IMicroserviceClient _hubClient;


		public SignalRHubChannel(ChannelInfo channelInfo, IMicroserviceClient hubClient)
		{
			_channelInfo = channelInfo ?? throw new ArgumentNullException(nameof(channelInfo));
			_hubClient = hubClient ?? throw new ArgumentNullException(nameof(hubClient));
		}


		public async Task OpenAsync(CancellationToken cancellationToken = default)
		{
			await _hubClient.LoginAsync(_channelInfo.PasswordIn, cancellationToken);
		}

		public async Task CloseAsync(CancellationToken cancellationToken = default)
		{
			await _hubClient.CloseChannelAsync();
			_hubClient.Dispose();
		}

		public async Task RunAsync(CancellationToken cancellationToken = default)
		{
			await _hubClient.RunChannelAsync();
		}

		public async Task StopAsync(CancellationToken cancellationToken = default)
		{
			await _hubClient.StopChannelAsync();
		}


		public bool TryConnect(out Exception error)
		{
			error = _hubClient.TryConnectAsync().Result;
			return (error == null);
		}

		public void CheckState()
		{
			_hubClient.CheckStateAsync().Wait();
		}

		public void Ping()
		{
			_hubClient.PingAsync().Wait();
		}

		public void Repair()
		{
			_hubClient.RepairAsync().Wait();
		}


		public void DeleteMessage(int msgLink)
		{
			_hubClient.DeleteMessageAsync(msgLink).Wait();
		}

		public void DeleteMessageBody(int msgLink)
		{
		}

		public void DeleteMessageContent(int contentLink)
		{
		}

		public void DeleteMessages(IEnumerable<int> msgLinks)
		{
			_hubClient.DeleteMessagesAsync(msgLinks).Wait();
		}

		public Message FindMessage(string msgGuid, string direction)
		{
			return _hubClient.FindMessageByGuidAsync(msgGuid, direction).Result;
		}

		public List<Message> GetMessages(string status, int? skip, int? take, out int totalCount)
		{
			(List<Message>, int) result = _hubClient.GetMessagesAsync(status, skip, take).Result;
			totalCount = result.Item2;
			return result.Item1;
		}

		public List<Message> GetLastMessages(string status, int? skip, int? take, out int totalCount)
		{
			(List<Message>, int) result = _hubClient.GetLastMessagesAsync(status, skip, take).Result;
			totalCount = result.Item2;
			return result.Item1;
		}

		public Message GetMessage(int msgLink)
		{
			return _hubClient.GetMessageAsync(msgLink).Result;
		}

		public MessageBody GetMessageBody(int msgLink)
		{
			Message msg = _hubClient.GetMessageAsync(msgLink).Result;
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
			_hubClient.SaveMessageAsync(msg).Wait();
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
					_hubClient.Dispose();
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
