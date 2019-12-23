namespace Microservices.Bus.Channels
{
	public class ChannelFactory : IChannelFactory
	{
		public ChannelFactory()
		{ }

		public IChannel CreateChannel(ChannelInfo channelInfo, IMicroserviceClient client)
		{
			return new SignalRHubChannel(channelInfo, client);
		}
	}
}
