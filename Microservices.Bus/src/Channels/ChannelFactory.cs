using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Bus.Channels
{
	public class ChannelFactory : IChannelFactory
	{
		public ChannelFactory()
		{
		}

		public IChannel CreateChannel(ChannelInfo channelInfo)
		{
			//channelInfo.Description.
			return new HubChannel(channelInfo);
		}
	}
}
