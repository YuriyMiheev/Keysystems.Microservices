using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Data;
using Microservices.Data.DAO;
using Microservices.Logging;

namespace Microservices.Bus.Channels
{
	public class MicroserviceChannel : IChannel, IDisposable
	{
		private readonly IChannelClient _client;
		private readonly ILogger _logger;


		public MicroserviceChannel(IChannelClient client, ILogger logger)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}


		public async Task OpenAsync(CancellationToken cancellationToken = default)
		{
			await _client.OpenChannelAsync(cancellationToken);
		}

		public async Task CloseAsync(CancellationToken cancellationToken = default)
		{
			await _client.CloseChannelAsync(cancellationToken);
			_client.Dispose();
		}

		public async Task RunAsync(CancellationToken cancellationToken = default)
		{
			await _client.RunChannelAsync(cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken = default)
		{
			await _client.StopChannelAsync(cancellationToken);
		}


		public bool TryConnect(out Exception error)
		{
			error = _client.TryConnectToChannelAsync().Result;
			return (error == null);
		}

		public void CheckState()
		{
			_client.CheckChannelStateAsync().Wait();
		}

		public void Ping()
		{
			_client.PingChannelAsync().Wait();
		}

		public void Repair()
		{
			_client.RepairChannelAsync().Wait();
		}


		public void DeleteMessage(int msgLink)
		{
			_client.DeleteMessageAsync(msgLink).Wait();
		}

		public void DeleteMessageBody(int msgLink)
		{
		}

		public void DeleteMessageContent(int contentLink)
		{
		}

		public void DeleteMessages(IEnumerable<int> msgLinks)
		{
			_client.DeleteMessagesAsync(msgLinks).Wait();
		}

		public Message FindMessage(string msgGuid, string direction)
		{
			return _client.FindMessageByGuidAsync(msgGuid, direction).Result;
		}

		public List<Message> GetMessages(string status, int? skip, int? take, out int totalCount)
		{
			(List<Message>, int) result = _client.GetMessagesAsync(status, skip, take).Result;
			totalCount = result.Item2;
			return result.Item1;
		}

		public List<Message> GetLastMessages(string status, int? skip, int? take, out int totalCount)
		{
			(List<Message>, int) result = _client.GetLastMessagesAsync(status, skip, take).Result;
			totalCount = result.Item2;
			return result.Item1;
		}

		public Message GetMessage(int msgLink)
		{
			return _client.GetMessageAsync(msgLink).Result;
		}

		public MessageBody GetMessageBody(int msgLink)
		{
			Message msg = _client.GetMessageAsync(msgLink).Result;
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
			_client.SaveMessageAsync(msg).Wait();
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
					_client.Dispose();
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
