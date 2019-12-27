using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Bus.Channels
{
	public interface IChannelManager
	{
		GroupInfo[] ChannelsGroups { get; }

		IChannelContext[] RuntimeChannels { get; }


		Task LoadChannelsAsync(CancellationToken cancellationToken = default);

		void TerminateChannel(string virtAddress);
	}
}
