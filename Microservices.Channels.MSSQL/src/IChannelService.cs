using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microservices.Channels.MSSQL.Adapters;
//using Microservices.Channels.MSSQL.Configuration;

namespace Microservices.Channels.MSSQL
{
	public interface IChannelService : Microservices.Channels.IChannelService
	{

		#region Properties
		MessageDataAdapter MessageDataAdapter { get; }
		#endregion

	}
}
