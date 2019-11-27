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


		//public  ConcurrentDictionary<string, ClientConnection> Connections
		//{
		//	get { return _connections; }
		//}

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

		public bool SendLogToClient(IDictionary<string, string> record)
		{
			if (_connections.Count == 0)
				return false;

			_connections.Values.AsParallel().ForAll(async conn =>
				{
					await conn.Client.Log(record);
				});
			return true;
		}

		public bool SendMessagesToClient(Message[] messages)
		{
			if (_connections.Count == 0)
				return false;

			_connections.Values.AsParallel().ForAll(async conn => 
				{
					await conn.Client.OutMessages(messages);
				});
			return true;
		}
	}
}
