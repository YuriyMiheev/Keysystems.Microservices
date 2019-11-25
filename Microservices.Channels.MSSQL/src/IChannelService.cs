using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

//using Microservices.Channels.MSSQL.Configuration;
using Microservices.Channels.MSSQL.Adapters;

namespace Microservices.Channels.MSSQL
{
	public interface IChannelService : Microservices.Channels.IChannelService, IHostedService
	{

		#region Properties
		MessageDataAdapter MessageDataAdapter { get; }
		#endregion

	}
}
