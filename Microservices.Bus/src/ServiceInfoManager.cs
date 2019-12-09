using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Bus
{
	public class ServiceInfoManager : IServiceInfoManager
	{
		private readonly ServiceInfo _serviceInfo;


		public ServiceInfoManager(ServiceInfo serviceInfo)
		{
			_serviceInfo = serviceInfo ?? throw new ArgumentNullException(nameof(serviceInfo));
		}


		#region Methods
		public ServiceInfo GetInfo()
		{
			return _serviceInfo;
		}

		public void UpdateInfo(ServiceInfoUpdateParams updateParams)
		{
		}

		public void SaveInfo(bool reinitService)
		{
		}
		#endregion

	}
}
