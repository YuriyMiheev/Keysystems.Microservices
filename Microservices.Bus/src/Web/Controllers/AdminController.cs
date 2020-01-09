using System;

using Microservices.Bus.Addins;
using Microservices.Bus.Channels;
using Microservices.Bus.Configuration;

using Microsoft.AspNetCore.Mvc;

namespace Microservices.Bus.Web.Controllers
{
	public partial class AdminController : Controller
	{
		private readonly ServiceInfo _serviceInfo;
		private readonly BusSettings _busSettings;
		private readonly IAddinManager _addinManager;
		private readonly IChannelManager _channelManager;


		public AdminController(ServiceInfo serviceInfo, BusSettings busSettings, IAddinManager addinManager, IChannelManager channelManager)
		{
			_serviceInfo = serviceInfo ?? throw new ArgumentNullException(nameof(serviceInfo));
			_busSettings = busSettings ?? throw new ArgumentNullException(nameof(busSettings));
			_addinManager = addinManager ?? throw new ArgumentNullException(nameof(addinManager));
			_channelManager = channelManager ?? throw new ArgumentNullException(nameof(channelManager));
		}


		[AcceptVerbs("GET")]
		//[AdminAccess]
		//[NoCache]
		public ActionResult Home()
		{
			this.ViewBag.Service = _serviceInfo.ToVmo();
			return View("Home");
		}
	}
}
