using System;

using Microservices.Bus.Addins;
using Microservices.Bus.Data;
using Microservices.Channels;

namespace Microservices.Bus.Channels
{
	public class ChannelContextFactory : IChannelContextFactory
	{
		private readonly IAddinManager _addinManager;
		private readonly IMicroserviceClientFactory _clientFactory;
		private readonly IChannelFactory _channelFactory;
		private readonly IBusDataAdapter _dataAdapter;


		public ChannelContextFactory(IAddinManager addinManager, IMicroserviceClientFactory clientFactory, IChannelFactory channelFactory, IBusDataAdapter dataAdapter)
		{
			_addinManager = addinManager ?? throw new ArgumentNullException(nameof(addinManager));
			_clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
			_channelFactory = channelFactory ?? throw new ArgumentNullException(nameof(channelFactory));
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="channelInfo"></param>
		/// <returns></returns>
		public IChannelContext CreateContext(ChannelInfo channelInfo)
		{
			if (channelInfo == null)
				throw new ArgumentNullException(nameof(channelInfo));

			IMicroserviceClient client = _clientFactory.CreateMicroserviceClient(channelInfo);
			MicroserviceDescription description = _addinManager.FindMicroservice(channelInfo.Provider);
			return new ProcessChannelContext(channelInfo, client, description, _channelFactory, _dataAdapter);
		}
	}
}
