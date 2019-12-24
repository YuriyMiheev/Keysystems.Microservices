using System.Threading.Tasks;

namespace Microservices.Bus.Channels
{
	public interface IChannelManager
	{
		GroupInfo[] ChannelsGroups { get; }

		IChannelContext[] RuntimeChannels { get; }


		Task LoadChannelsAsync();

		void TerminateChannel(string virtAddress);
	}
}
