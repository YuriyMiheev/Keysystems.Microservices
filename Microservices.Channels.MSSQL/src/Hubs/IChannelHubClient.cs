using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microservices.Channels.Hubs;

namespace Microservices.Channels.MSSQL.Hubs
{
	public interface IChannelHubClient : IChannelHubCallback
	{
	}
}
