using System;

using AutoMapper;

using DAO = Microservices.Bus.Data.DAO;

namespace Microservices.Bus
{
	public static class ServiceInfoExtensions
	{
		//private readonly static IMapper mapper;

		//static ServiceInfoExtensions()
		//{
		//	var config = new MapperConfiguration(cfg =>
		//		{
		//			cfg.CreateMap<ServiceInfo, ServiceInfo>();
		//		});
		//	mapper = config.CreateMapper();
		//}

		//public static void CloneTo(this ServiceInfo src, ServiceInfo dst)
		//{
		//	mapper.Map<ServiceInfo, ServiceInfo>(src, dst);
		//}

		//public static ServiceInfo ToObj(this DAO.ServiceInfo dao)
		//{
		//	if (dao == null)
		//		return null;

		//	var obj = new ServiceInfo();
		//	dao.CloneTo(obj);

		//	return obj;
		//}

		public static DAO.ServiceInfo ToDao(this ServiceInfo obj)
		{
			if (obj == null)
				return null;

			var dao = new DAO.ServiceInfo();
			dao.AuthorizeEnabled = (obj.AuthorizeEnabled == false ? new Nullable<bool>() : obj.AuthorizeEnabled);
			dao.DebugEnabled = (obj.DebugEnabled == false ? new Nullable<bool>() : obj.DebugEnabled);
			dao.ExternalAddress = (String.IsNullOrEmpty(obj.ExternalAddress) ? null : obj.ExternalAddress);
			dao.InstanceID = obj.InstanceID;
			dao.InternalAddress = (String.IsNullOrEmpty(obj.InternalAddress) ? null : obj.InternalAddress);
			dao.LINK = obj.LINK;
			dao.MaxUploadSize = obj.MaxUploadSize;
			dao.Online = (obj.Online == false ? new Nullable<bool>() : obj.Online);
			//dao.Properties = obj.Properties.Select(prop => prop.ToDao(dao)).ToList();
			dao.ServiceName = obj.ServiceName;
			dao.ShutdownReason = (String.IsNullOrEmpty(obj.ShutdownReason) ? null : obj.ShutdownReason);
			dao.ShutdownTime = obj.ShutdownTime;
			dao.StartTime = obj.StartTime;
			dao.Version = obj.Version;

			return dao;
		}

		public static void CopyTo(this DAO.ServiceInfo dao, ServiceInfo obj)
		{
			#region Validate parameters
			if (dao == null)
				throw new ArgumentNullException("dao");

			if (obj == null)
				throw new ArgumentNullException("obj");
			#endregion

			obj.AuthorizeEnabled = (dao.AuthorizeEnabled == null ? false : dao.AuthorizeEnabled.Value);
			obj.DebugEnabled = (dao.DebugEnabled == null ? false : dao.DebugEnabled.Value);
			obj.ExternalAddress = dao.ExternalAddress;
			obj.InstanceID = dao.InstanceID;
			obj.InternalAddress = dao.InternalAddress;
			obj.LINK = dao.LINK;
			obj.MaxUploadSize = dao.MaxUploadSize;
			obj.Online = (dao.Online == null ? false : dao.Online.Value);
			//obj.Properties = dao.Properties.Select(prop => prop.ToObj()).ToArray();
			obj.ServiceName = dao.ServiceName;
			obj.ShutdownReason = dao.ShutdownReason;
			obj.ShutdownTime = dao.ShutdownTime;
			obj.StartTime = (dao.StartTime ?? DateTime.MinValue);
			obj.Version = dao.Version;
		}
	}
}
