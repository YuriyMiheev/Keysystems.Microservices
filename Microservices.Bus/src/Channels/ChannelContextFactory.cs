using System;
using Microservices.Bus.Data;

namespace Microservices.Bus.Channels
{
	public class ChannelContextFactory : IChannelContextFactory
	{
		private readonly IChannelFactory _factory;
		private readonly IBusDataAdapter _dataAdapter;


		public ChannelContextFactory(IChannelFactory factory, IBusDataAdapter dataAdapter)
		{
			_factory = factory ?? throw new ArgumentNullException(nameof(factory));
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
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
			return new ProcessChannelContext(channelInfo, _factory, _dataAdapter);
		}
	}
}
