using Microservices.Bus.Channels;

namespace Microservices.Bus.Addins
{
	public interface IAddinManager
	{
		ChannelDescription[] RegisteredChannels { get; }

		void LoadAddins();
	}
}
