using System;

namespace Microservices.Channels.Hubs
{
	public interface IHubConnection : IDisposable
	{

		string ConnectionId { get; }

		IChannelHubClient Client { get; }

	}
}
