using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microservices.Channels.MSSQL.Adapters;
using Microservices.Channels.MSSQL.Configuration;

namespace Microservices.Channels.MSSQL
{
	public interface IChannelService : Microservices.Channels.IChannelService
	{

		#region Settings
		ChannelConfigFileSettings ChannelSettings { get; }

		ServiceConfigFileSettings ServiceSettings { get; }
		#endregion


		#region Properties
		MessageDataAdapter MessageDataAdapter { get; }
		#endregion

	}
}
