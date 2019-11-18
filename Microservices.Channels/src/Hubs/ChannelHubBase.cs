using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

namespace Microservices.Common.Hubs
{
	public abstract class ChannelHubBase<TChannelService, TClient> : Hub<TClient> where TClient: class
	{
		protected ChannelHubBase(TChannelService service)
		{
			this.ChannelService = service ?? throw new ArgumentNullException(nameof(service));
		}


		protected TChannelService ChannelService { get; private set; }


		public abstract string Connect(string accessKey);

		public abstract void StartService();

		public abstract void StopService();

		public abstract IDictionary<string, string> GetSettings();

		public abstract void SetSettings(IDictionary<string, string> settings);

		public abstract void SaveSettings();

	}
}
