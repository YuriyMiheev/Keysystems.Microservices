using System;

using Microservices.Bus.Addins;
using Microservices.Bus.Data;
using Microservices.Bus.Logging;
using Microservices.Channels;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Фабрика создания каналов.
	/// </summary>
	public class ChannelFactory : IChannelFactory
	{
		private readonly IAddinManager _addinManager;
		private readonly IBusDataAdapter _dataAdapter;
		private readonly ILogger _logger;


		public ChannelFactory(IAddinManager addinManager, IBusDataAdapter dataAdapter, ILogger logger)
		{
			_addinManager = addinManager ?? throw new ArgumentNullException(nameof(addinManager));
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

			var channelStatus = new ChannelStatus();
			IChannelClient client = new SignalRHubClient(channelInfo.SID, channelStatus, _logger);
			//IMicroserviceClient client = new GrpcClient(channelInfo.SID, channelStatus, _logger);
			IAddinDescription description = _addinManager.FindDescription(channelInfo.Provider);
			return new ProcessChannelContext(description, channelInfo, client, _dataAdapter, _logger, CreateChannel);
		}

		private IChannel CreateChannel(ChannelInfo channelInfo, IChannelClient client, ILogger logger)
		{
			return new MicroserviceChannel(client, logger);
		}
	}
}
