using Microservices.Channels;

namespace Microservices.Bus.Channels
{
	public class MicroserviceClientFactory : IMicroserviceClientFactory
	{
		public IMicroserviceClient CreateMicroserviceClient(ChannelInfo channelInfo)
		{
			IMicroserviceClient client = new ChannelHubClient();
			client.Url = channelInfo.SID;
			client.Status = new ChannelStatus();
			return client;
		}
	}
}
