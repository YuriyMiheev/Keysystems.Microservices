using Microservices.Channels;

namespace Microservices.Bus.Channels
{
	public class MicroserviceClientFactory : IMicroserviceClientFactory
	{
		public IMicroserviceClient CreateMicroserviceClient(ChannelInfo channelInfo)
		{
			return new SignalRHubClient(channelInfo.SID, new ChannelStatus());
		}
	}
}
