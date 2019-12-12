using System;

using Microservices.Channels;
using Microservices.Channels.Hubs;
using Microservices.Configuration;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using MSSQL.Microservice.Hubs;

namespace MSSQL.Microservice.Controllers
{
	public class ChannelController : Controller
	{
		private readonly IChannelService _channelService;
		private readonly IAppSettingsConfig _appConfig;
		private readonly IHubContext<ChannelHub, IChannelHubClient> _hubContext;


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
