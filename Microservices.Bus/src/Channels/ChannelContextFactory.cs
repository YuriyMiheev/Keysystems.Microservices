using System;

using Microservices.Bus.Addins;
using Microservices.Bus.Data;

namespace Microservices.Bus.Channels
{
	public class ChannelContextFactory : IChannelContextFactory
	{
		private readonly IAddinManager _addinManager;
		private readonly IChannelFactory _factory;
		private readonly IBusDataAdapter _dataAdapter;


		public ChannelContextFactory(IAddinManager addinManager, IChannelFactory factory, IBusDataAdapter dataAdapter)
		{
			_addinManager = addinManager ?? throw new ArgumentNullException(nameof(addinManager));
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
			MicroserviceDescription description = _addinManager.FindMicroservice(channelInfo.Provider);
			return new ProcessChannelContext(channelInfo, description, _factory, _dataAdapter);
		}
	}
}
