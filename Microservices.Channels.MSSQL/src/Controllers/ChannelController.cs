using System;

using Microservices.Channels.MSSQL.Hubs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Microservices.Channels.MSSQL.Controllers
{
	public class ChannelController : Controller
	{
		private IChannelService _channelService;
		private IHubContext<ChannelHub, IChannelHubClient> _hubContext;


		public ChannelController(IChannelService channelService, IHubContext<ChannelHub, IChannelHubClient> hubContext)
		{
			_channelService = channelService ?? throw new ArgumentNullException(nameof(channelService));
			_hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
		}

		public IActionResult Info()
		{
			var settings = _channelService.GetAppSettings().Values;
			//_hubContext.Clients.All.ReceiveLog();
			return Json(settings);
		}
	}
}
