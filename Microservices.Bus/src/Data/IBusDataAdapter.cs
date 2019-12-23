using System.Collections.Generic;

using Microservices.Bus.Channels;
using Microservices.Data;

namespace Microservices.Bus.Data
{
	public interface IBusDataAdapter : IMessageDataAdapter
	{
		List<DAO.ServiceInfo> GetServiceInstances();

		void SaveServiceInfo(ServiceInfo serviceInfo);


		List<GroupInfo> GetGroups();

		void SaveGroup(GroupInfo groupInfo);

		void DeleteGroup(GroupInfo groupInfo);

		List<GroupChannelMap> GetGroupMap();

		void SaveGroupMap(GroupChannelMap map);

		void DeleteGroupMap(GroupChannelMap map);


		void SaveChannelInfo(ChannelInfo channelInfo);

		List<ChannelInfo> GetChannels();
	}
}
