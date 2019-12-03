using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservices.Channels.Hubs
{
	public class HubClientConnections : IHubClientConnections
	{
		private ConcurrentDictionary<string, HubClientConnection> _connections;


		public HubClientConnections()
		{
			_connections = new ConcurrentDictionary<string, HubClientConnection>();
		}



		public void Add(HubClientConnection connection)
		{
			_connections.TryAdd(connection.ConnectionId, connection);
		}

		public bool TryGet(string connectionId, out HubClientConnection client)
		{
			return _connections.TryGetValue(connectionId, out client);
		}

		public bool TryRemove(string connectionId, out HubClientConnection connection)
		{
			return _connections.TryRemove(connectionId, out connection);
		}

		public bool SendLogToClient(IDictionary<string, object> record)
		{
			if (_connections.Count == 0)
				return false;

			_connections.Values.AsParallel().ForAll(async conn =>
				{
					await conn.Client.ReceiveLog(record);
				});
			return true;
		}

		public bool SendMessagesToClient(Message[] messages)
		{
			if (_connections.Count == 0)
				return false;

			_connections.Values.AsParallel().ForAll(async conn => 
				{
					await conn.Client.ReceiveMessages(messages);
				});
			return true;
		}

		public void SendStatusToClient(string statusName, object statusValue)
		{
			_connections.Values.AsParallel().ForAll(async conn =>
				{
					await conn.Client.ReceiveStatus(statusName, statusValue);
				});
		}

	}
}
