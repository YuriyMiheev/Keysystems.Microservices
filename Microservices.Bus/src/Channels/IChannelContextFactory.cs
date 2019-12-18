namespace Microservices.Bus.Channels
{
	public interface IChannelContextFactory
	{
		IChannelContext CreateContext(ChannelInfo channelInfo);
	}
}
