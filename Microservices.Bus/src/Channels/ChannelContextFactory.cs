using System;

namespace Microservices.Bus.Channels
{
	public class ChannelContextFactory : IChannelContextFactory
	{
		private readonly IChannelFactory _factory;


		public ChannelContextFactory(IChannelFactory factory)
		{
			_factory = factory ?? throw new ArgumentNullException(nameof(factory));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="channelInfo"></param>
		/// <returns></returns>
		public IChannelContext CreateContext(ChannelInfo channelInfo)
		{
			#region Validate parameters
			if (channelInfo == null)
				throw new ArgumentNullException(nameof(channelInfo));
			#endregion

			//channelInfo.Description.Type
			return new ProcessChannelContext(channelInfo, _factory);
		}
	}
}
