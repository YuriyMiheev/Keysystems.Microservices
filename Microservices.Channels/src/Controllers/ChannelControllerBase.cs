using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

using Microservices.Common.Hubs;

namespace Microservices.Common.Controllers
{
	public abstract class ChannelControllerBase<TChannelService> : Controller where TChannelService : class
	{
		protected ChannelControllerBase(TChannelService service)
		{
			this.ChannelService = service ?? throw new ArgumentNullException(nameof(service));
		}


		protected TChannelService ChannelService { get; private set; }

	}
}
