namespace Microservices.Bus.Channels
{
	public interface IMicroserviceClientFactory
	{
		IMicroserviceClient CreateMicroserviceClient(ChannelInfo channelInfo);
	}
}
