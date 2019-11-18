using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Channels.Hubs
{
	public interface IChannelHubCallback
	{

		#region Messages
		Task NewMessage(Message msg);
		#endregion


		#region Logging
		Task ReceiveLog(string traceId, string channel, string logLevel, string text);
		#endregion

	}
}
