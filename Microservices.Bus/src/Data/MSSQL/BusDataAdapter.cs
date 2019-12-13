using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microservices.Data;
using Microservices.Data.MSSQL;

namespace Microservices.Bus.Data.MSSQL
{
	public class BusDataAdapter : MessageDataAdapterBase, IBusDataAdapter
	{

		#region Ctor
		/// <summary>
		/// Создание экземпляра.
		/// </summary>
		/// <param name="database"></param>
		public BusDataAdapter(IDatabase database)
			: base(database)
		{ }

		/// <summary>
		/// Создание экземпляра.
		/// </summary>
		/// <param name="dbContext"></param>
		public BusDataAdapter(DbContext dbContext)
				: base(dbContext)
		{ }
		#endregion


		protected override MessageBodyStreamBase ConstructMessageBodyStream(int msgLink, UnitOfWork work, DataStreamMode mode, Encoding encoding)
		{
			return new MessageBodyStream(this.DbContext, work, mode, msgLink, encoding);
		}

		protected override MessageContentStreamBase ConstructMessageContentStream(int contentLink, UnitOfWork work, DataStreamMode mode, Encoding encoding)
		{
			return new MessageContentStream(this.DbContext, work, mode, contentLink, encoding);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<DAO.ServiceInfo> GetServiceInstances()
		{
			using (IDataQuery dataQuery = OpenQuery())
			{
				return dataQuery.Open<DAO.ServiceInfo>().List().ToList();
			}
		}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="instanceId"></param>
		///// <returns></returns>
		//public DAO.ServiceInfo GetServiceInfo(string instanceId)
		//{
		//	using (IDataQuery dataQuery = OpenQuery())
		//	{
		//		return dataQuery.Open<DAO.ServiceInfo>()
		//			.Where(si => si.InstanceID == instanceId)
		//			.SingleOrDefault();
		//	}
		//}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceInfo"></param>
		public void SaveServiceInfo(ServiceInfo serviceInfo)
		{
			#region Validate parameters
			if (serviceInfo == null)
				throw new ArgumentNullException("serviceInfo");
			#endregion

			DAO.ServiceInfo dao = serviceInfo.ToDao();

			using (UnitOfWork work = BeginWork())
			{
				if (dao.LINK == 0)
					work.Save(dao);
				else
					work.Update<DAO.ServiceInfo>(ref dao);

				work.End();
			}

			dao.CopyTo(serviceInfo);
		}
	}
}
