namespace Microservices.Bus.Channels
{
	public interface IChannelManager
	{
		GroupInfo[] ChannelsGroups { get; }

		IChannelContext[] RuntimeChannels { get; }


		void LoadChannels();

		void TerminateChannel(string virtAddress);
	}
}
