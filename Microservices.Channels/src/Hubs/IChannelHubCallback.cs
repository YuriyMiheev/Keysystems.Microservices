using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Channels.Hubs
{
	public interface IChannelHubCallback
	{

		#region Messages
		Task NewMessages(Message[] messages);
		#endregion


		#region Logging
		Task ReceiveLog(IDictionary<string, string> logRecord);
		#endregion

	}
}
