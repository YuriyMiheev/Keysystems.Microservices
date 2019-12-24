﻿namespace Microservices.Bus.Channels
{
	public interface IChannelFactory
	{
		IChannelContext CreateChannel(ChannelInfo channelInfo);
	}
}
