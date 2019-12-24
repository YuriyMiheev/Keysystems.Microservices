namespace Microservices.Bus.Channels
{
	public class ChannelFactory : IChannelFactory
	{
		public ChannelFactory()
		{ }

		public IChannel CreateChannel(ChannelInfo channelInfo, IMicroserviceClient client)
		{
			//if (channelInfo.IsSystem())
			//	return new SystemChannel();
			//else
				return new MicroserviceChannel(channelInfo, client);
		}
	}
}
