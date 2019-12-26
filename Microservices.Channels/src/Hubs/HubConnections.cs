using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.Channels.Hubs
{
	public class HubConnections : IHubConnections
	{
		private ConcurrentDictionary<string, IHubConnection> _connections;


		public HubConnections()
		{
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

		public bool SendLogToClient(IDictionary<string, object> record)
		{
			if (_connections.Count == 0)
				return false;

			_connections.Values.ToList().ForEach(async conn =>
			//_connections.Values.AsParallel().ForAll(async conn =>
			{
				await conn.Client.ReceiveLog(record);
			});
			return true;
		}

		public bool SendMessagesToClient(Message[] messages)
		{
			if (_connections.Count == 0)
				return false;

			_connections.Values.ToList().ForEach(async conn =>
			//_connections.Values.AsParallel().ForAll(async conn => 
			{
				await conn.Client.ReceiveMessages(messages);
			});
			return true;
		}

		public void SendStatusToClient(string statusName, object statusValue)
		{
			_connections.Values.ToList().ForEach(async conn =>
			//_connections.Values.AsParallel().ForAll(async conn =>
			{
				await conn.Client.ReceiveStatus(statusName, statusValue);
			});
		}

	}
}
