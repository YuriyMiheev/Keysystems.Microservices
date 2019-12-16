using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Bus.Channels
{
	public interface IChannelManager
	{
		GroupInfo[] ChannelsGroups { get; }

		ChannelInfo[] RuntimeChannels { get; }


		void LoadChannels();
	}
}
