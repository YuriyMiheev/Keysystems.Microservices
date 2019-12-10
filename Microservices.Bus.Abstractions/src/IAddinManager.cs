using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Bus
{
	public interface IAddinManager
	{
		ChannelDescription[] RegisteredChannels { get; }

		void LoadAddins();
	}
}
