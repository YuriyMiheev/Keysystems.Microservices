using System.Collections.Generic;

namespace Microservices.Channels.Hubs
{
	public interface IHubConnections
	{
		void Add(HubConnection connection);

		bool TryGet(string connectionId, out HubConnection connection);

		bool TryRemove(string connectionId, out HubConnection connection);

		bool SendLogToClient(IDictionary<string, object> record);

		bool SendMessagesToClient(Message[] messages);

		void SendStatusToClient(string statusName, object statusValue);
	}
}
