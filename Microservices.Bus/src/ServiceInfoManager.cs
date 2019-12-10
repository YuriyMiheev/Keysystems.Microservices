using System;

using Microservices.Bus.Data;

namespace Microservices.Bus
{
	public class ServiceInfoManager : IServiceInfoManager
	{
		private readonly ServiceInfo _serviceInfo;
		private readonly IBusDataAdapter _dataAdapter;


		public ServiceInfoManager(ServiceInfo serviceInfo, IBusDataAdapter dataAdapter)
		{
			_serviceInfo = serviceInfo ?? throw new ArgumentNullException(nameof(serviceInfo));
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
		}


		#region Methods
		public ServiceInfo GetInfo()
		{
			return _serviceInfo;
		}

		public void UpdateInfo(ServiceInfoUpdateParams updateParams)
		{
			_serviceInfo.Online = updateParams.Online;
			_serviceInfo.InternalAddress = updateParams.InternalAddress;
			_serviceInfo.ExternalAddress = updateParams.ExternalAddress;
		}

		public void SaveInfo(bool reinitService)
		{
			//_dataAdapter.SaveInfo(_serviceInfo);
		}
		#endregion

	}
}
