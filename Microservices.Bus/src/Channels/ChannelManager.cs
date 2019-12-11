using System.Collections.Generic;
using System.Linq;

namespace Microservices.Bus.Channels
{
	public class ChannelManager : IChannelManager
	{
		//private readonly ChannelFactory _factory;
		private readonly List<GroupInfo> _channelsGroups;
		private readonly List<IChannel> _channels;


		public ChannelManager()
		{
			//_factory = new ChannelFactory();
			_channelsGroups = new List<GroupInfo>();
			_channels = new List<IChannel>();
		}


		#region Properties
		/// <summary>
		/// {Get} Список групп каналов.
		/// </summary>
		public GroupInfo[] ChannelsGroups
		{
			get { return _channelsGroups.ToArray(); }
		}

		/// <summary>
		/// {Get} Список созданных каналов.
		/// </summary>
		public ChannelInfo[] RuntimeChannels
		{
			get { return _channels.Select(channel => channel.GetInfo()).OrderBy(channelInfo => channelInfo.LINK).ToArray(); }
		}
		#endregion


		public void LoadChannels()
		{
		}
	}
}
