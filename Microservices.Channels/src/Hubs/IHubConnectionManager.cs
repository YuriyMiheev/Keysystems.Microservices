using System.Collections.Generic;

namespace Microservices.Channels.Hubs
{
	public interface IHubConnectionManager
	{
		void Add(IHubConnection connection);

		bool TryGet(string connectionId, out IHubConnection connection);

		bool TryRemove(string connectionId, out IHubConnection connection);

		bool SendLogToClient(IDictionary<string, object> logRecord);

		bool SendMessagesToClient(Message[] messages);

		void SendStatusToClient(ChannelStatus status);
	}
}
