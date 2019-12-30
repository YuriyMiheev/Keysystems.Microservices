using System;
using System.Collections.Generic;

namespace Microservices.Channels
{
	public static class ChannelStatusExtensions
	{
		public static IDictionary<string, object> ToDict(this ChannelStatus status)
		{
			if (status == null)
				throw new ArgumentNullException(nameof(status));

			return new Dictionary<string, object>
			{
				{ nameof(status.Created), status.Created },
				{ nameof(status.Opened), status.Opened },
				{ nameof(status.Running), status.Running },
				{ nameof(status.Online), status.Online },
				{ nameof(status.Error), status.Error }
			};
		}
	}
}
