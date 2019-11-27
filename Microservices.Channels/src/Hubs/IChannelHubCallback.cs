using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Channels.Hubs
{
	public interface IChannelHubCallback
	{

		#region Messages
		Task OutMessages(Message[] messages);
		#endregion


		#region Logging
		Task Log(IDictionary<string, string> record);
		#endregion

	}
}
