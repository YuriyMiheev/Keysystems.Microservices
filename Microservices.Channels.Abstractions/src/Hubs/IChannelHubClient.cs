using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservices.Channels.Hubs
{
	public interface IChannelHubClient
	{

		Task ReceiveMessages(Message[] messages);

		Task ReceiveStatus(string statusName, object statusValue);
		
		Task ReceiveLog(IDictionary<string, object> record);

	}
}
