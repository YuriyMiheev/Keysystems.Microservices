using System;

using Microservices.Bus.Addins;
using Microservices.Bus.Data;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Фабрика создания каналов.
	/// </summary>
	public class ChannelFactory : IChannelFactory
	{
		private readonly IAddinManager _addinManager;
		private readonly IBusDataAdapter _dataAdapter;


		public ChannelFactory(IAddinManager addinManager, IBusDataAdapter dataAdapter)
		{
			_addinManager = addinManager ?? throw new ArgumentNullException(nameof(addinManager));
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="channelInfo"></param>
		/// <returns></returns>
		public IChannelContext CreateChannel(ChannelInfo channelInfo)
		{
			if (channelInfo == null)
				throw new ArgumentNullException(nameof(channelInfo));

			IMicroserviceClient client = new SignalRHubClient(channelInfo.SID);
			IAddinDescription description = _addinManager.FindDescription(channelInfo.Provider);
			return new ProcessChannelContext(description, channelInfo, client, _dataAdapter, CreateChannel);
		}

		private IChannel CreateChannel(ChannelInfo channelInfo, IMicroserviceClient client)
		{
			return new MicroserviceChannel(client);
		}
	}
}
