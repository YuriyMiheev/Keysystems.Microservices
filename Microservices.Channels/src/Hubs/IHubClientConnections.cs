using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Channels.Hubs
{
	public interface IHubClientConnections
	{
		void Add(HubClientConnection connection);

		bool TryGet(string connectionId, out HubClientConnection connection);

		bool TryRemove(string connectionId, out HubClientConnection connection);

		bool SendLogToClient(IDictionary<string, string> record);

		bool SendMessagesToClient(Message[] messages);
	}
}
