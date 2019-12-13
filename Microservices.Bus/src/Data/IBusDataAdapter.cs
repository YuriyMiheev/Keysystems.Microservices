using System.Collections.Generic;

using Microservices.Data;

namespace Microservices.Bus.Data
{
	public interface IBusDataAdapter : IMessageDataAdapter
	{
		List<DAO.ServiceInfo> GetServiceInstances();

		//DAO.ServiceInfo GetServiceInfo(string instanceId);

		void SaveServiceInfo(ServiceInfo serviceInfo);
	}
}
