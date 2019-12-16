using System.Collections.Generic;

using Microservices.Bus.Channels;
using Microservices.Data;

namespace Microservices.Bus.Data
{
	public interface IBusDataAdapter : IMessageDataAdapter
	{
		List<DAO.ServiceInfo> GetServiceInstances();

		void SaveServiceInfo(ServiceInfo serviceInfo);


		List<GroupInfo> GetChannelsGroups();

		void SaveChannelsGroup(GroupInfo groupInfo);

		void DeleteChannelsGroup(GroupInfo groupInfo);

		List<GroupChannelMap> GetGroupsChannelsMap();

		void SaveGroupsChannelsMap(GroupChannelMap map);

		void DeleteGroupsChannelsMap(GroupChannelMap map);


		void SaveChannel(ChannelInfo channelInfo);

		List<ChannelInfo> GetChannels();
	}
}
