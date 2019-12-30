using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Microservices.Channels.Logging;

namespace Microservices.Channels.Hubs
{
	public class HubConnectionManager : IHubConnectionManager
	{
		private readonly ILogger _logger;
		private ConcurrentDictionary<string, IHubConnection> _connections;


		public HubConnectionManager(ILogger logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_connections = new ConcurrentDictionary<string, IHubConnection>();
		}



		public void Add(IHubConnection connection)
		{
			_connections.TryAdd(connection.ConnectionId, connection);
		}

		public bool TryGet(string connectionId, out IHubConnection connection)
		{
			return _connections.TryGetValue(connectionId, out connection);
		}

		public bool TryRemove(string connectionId, out IHubConnection connection)
		{
			return _connections.TryRemove(connectionId, out connection);
		}

		public bool SendLogToClient(IDictionary<string, object> logRecord)
		{
			if (_connections.Count == 0)
				return false;

			//foreach (IHubConnection conn in _connections.Values)
			//_connections.Values.AsParallel().ForAll(async conn =>
			_connections.Values.ToList().ForEach(async conn =>
				{
					await conn.Client.ReceiveLog(logRecord);
				});
			return true;
		}

		public bool SendMessagesToClient(Message[] messages)
		{
			if (_connections.Count == 0)
				return false;

			_logger.LogTrace($"Send-> Messages={messages.Length}");

			//_connections.Values.AsParallel().ForAll(async conn => 
			_connections.Values.ToList().ForEach(async conn =>
				{
					await conn.Client.ReceiveMessages(messages);
				});
			return true;
		}

		public void SendStatusToClient(ChannelStatus status)
		{
			_logger.LogTrace($"Send-> Status={status}");

			//_connections.Values.AsParallel().ForAll(async conn =>
			_connections.Values.ToList().ForEach(async conn =>
				{
					await conn.Client.ReceiveStatus(status.ToDict());
				});
		}

	}
}
