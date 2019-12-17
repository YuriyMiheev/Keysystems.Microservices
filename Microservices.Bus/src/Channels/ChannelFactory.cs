using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Bus.Channels
{
	public class ChannelFactory : IChannelFactory
	{
		public ChannelFactory(IServiceProvider serviceProvider)
		{

		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="channelInfo"></param>
		/// <returns></returns>
		public IChannel CreateChannel(ChannelInfo channelInfo)
		{
			#region Validate parameters
			if (channelInfo == null)
				throw new ArgumentNullException(nameof(channelInfo));
			#endregion

			return new ChannelHubHost(channelInfo);
		}
	}
}
