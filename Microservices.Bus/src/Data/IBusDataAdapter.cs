using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Bus.Channels;
using Microservices.Data;

namespace Microservices.Bus.Data
{
	/// <summary>
	/// Адаптер БД шины.
	/// </summary>
	public interface IBusDataAdapter : IMessageDataAdapter
	{
		List<DAO.ServiceInfo> GetServiceInstances();

		void SaveServiceInfo(ServiceInfo serviceInfo);


		Task<List<GroupInfo>> GetGroupsAsync(CancellationToken cancellationToken = default);

		void SaveGroup(GroupInfo groupInfo);

		void DeleteGroup(GroupInfo groupInfo);

		List<GroupChannelMap> GetGroupMap();

		void SaveGroupMap(GroupChannelMap map);

		void DeleteGroupMap(GroupChannelMap map);


		void SaveChannelInfo(ChannelInfo channelInfo);

		Task<List<ChannelInfo>> GetChannelsAsync(CancellationToken cancellationToken = default);
	}
}
