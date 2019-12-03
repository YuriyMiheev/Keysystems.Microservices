using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Channels.Client
{
	public interface IChannelHubClient
	{

		#region Properties
		UriBuilder HubUrl { get; }

		IWebProxy WebProxy { get; set; }

		bool IsConnected { get; }

		IDictionary<string, object> ChannelInfo { get; }

		ChannelStatus ChannelStatus { get; }
		#endregion


		#region Events
		event Action<IChannelHubClient> Connected;

		event Func<IChannelHubClient, Exception, Task> Reconnecting;

		event Func<IChannelHubClient, string, Task> Reconnected;

		event Func<IChannelHubClient, Exception, Task> Disconnected;
		#endregion

	}
}
