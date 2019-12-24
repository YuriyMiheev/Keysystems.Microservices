namespace Microservices.Bus.Channels
{
	public interface IChannelFactory
	{
		IChannel CreateChannel(ChannelInfo channelInfo, IMicroserviceClient client);
	}
}
