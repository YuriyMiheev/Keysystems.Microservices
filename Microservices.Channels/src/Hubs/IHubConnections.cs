using System.Collections.Generic;

namespace Microservices.Channels.Hubs
{
	public interface IHubConnections
	{
		void Add(IHubConnection connection);

		bool TryGet(string connectionId, out IHubConnection connection);

		bool TryRemove(string connectionId, out IHubConnection connection);

		bool SendLogToClient(IDictionary<string, object> record);

		bool SendMessagesToClient(Message[] messages);

		void SendStatusToClient(string statusName, object statusValue);
	}
}
