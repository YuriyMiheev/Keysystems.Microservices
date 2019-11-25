using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

using Microservices.Channels.Hubs;
using Microservices.Channels.MSSQL.Hubs;

namespace Microservices.Channels.MSSQL.Controllers
{
	public class ChannelController : Controller
	{
		private IChannelService _service;
		private IHubContext<ChannelHub, IChannelHubClient> _hubContext;


		public ChannelController(IChannelService service, IHubContext<ChannelHub, IChannelHubClient> hubContext)
		{
			_service = service ?? throw new ArgumentNullException(nameof(service));
			_hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
		}

		public IActionResult Info()
		{
			var settings = _service.InfoSettings.GetAppSettings().Values;
			//_hubContext.Clients.All.ReceiveLog();
			return Json(settings);
		}
	}
}
