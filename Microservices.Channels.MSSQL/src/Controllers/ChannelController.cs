using System;

using Microservices.Channels.MSSQL.Hubs;
using Microservices.Configuration;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Microservices.Channels.MSSQL.Controllers
{
	public class ChannelController : Controller
	{
		private IChannelService _channelService;
		private IAppSettingsConfig _appConfig;
		private IHubContext<ChannelHub, IChannelHubClient> _hubContext;


		public ChannelController(IChannelService channelService, IAppSettingsConfig appConfig, IHubContext<ChannelHub, IChannelHubClient> hubContext)
		{
			_channelService = channelService ?? throw new ArgumentNullException(nameof(channelService));
			_appConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
			_hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
		}

		public IActionResult Info()
		{
			var settings = _appConfig.GetAppSettings();
			return Json(settings);
		}
	}
}
