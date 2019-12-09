using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Bus
{
	public interface IServiceInfoManager
	{

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		ServiceInfo GetInfo();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="updateParams"></param>
		void UpdateInfo(ServiceInfoUpdateParams updateParams);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reinitService"></param>
		void SaveInfo(bool reinitService);
		#endregion

	}
}
