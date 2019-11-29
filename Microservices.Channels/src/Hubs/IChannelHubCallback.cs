using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Channels.Hubs
{
	public interface IChannelHubCallback
	{

		Task ReceiveMessages(Message[] messages);

		Task ReceiveStatus(IDictionary<string, object> status);
		
		Task ReceiveLog(IDictionary<string, string> record);

	}
}
