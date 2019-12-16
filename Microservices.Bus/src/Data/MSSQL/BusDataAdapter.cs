using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microservices.Bus.Channels;
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
		public BusDataAdapter(IBusDatabase database)
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


		public List<ChannelInfo> GetChannels()
		{
			using (IDataQuery dataQuery = OpenQuery())
			{
				return dataQuery.Open<DAO.ChannelInfo>().List().Select(dao => dao.ToObj()).ToList();
			}
		}

		public void SaveChannel(ChannelInfo channelInfo)
		{
			#region Validate parameters
			if (channelInfo == null)
				throw new ArgumentNullException("channelInfo");
			#endregion

			DAO.ChannelInfo dao = channelInfo.ToDao();

			using (UnitOfWork work = BeginWork())
			{
				if (dao.LINK == 0)
					work.Save(dao);
				else
					work.Update<DAO.ChannelInfo>(ref dao);

				work.End();
			}

			dao.CloneTo(channelInfo);
		}


		public List<GroupInfo> GetChannelsGroups()
		{
			using (IDataQuery dataQuery = OpenQuery())
			{
				List<GroupInfo> groups = dataQuery.Open<DAO.GroupInfo>().List().Select(dao => dao.ToObj()).ToList();
				List<GroupChannelMap> map = dataQuery.Open<DAO.GroupChannelMap>().List().Select(dao => dao.ToObj()).ToList();
				foreach (GroupInfo group in groups)
				{
					group.Channels = map.Where(x => x.GroupLINK == group.LINK).Where(x => x.ChannelLINK != null).Select(x => x.ChannelLINK.Value).ToArray();
				}

				return groups;
			}
		}

		public void SaveChannelsGroup(GroupInfo groupInfo)
		{
			#region Validate parameters
			if (groupInfo == null)
				throw new ArgumentNullException("groupInfo");
			#endregion

			DAO.GroupInfo dao = groupInfo.ToDao();

			using (UnitOfWork work = BeginWork())
			{
				if (dao.LINK == 0)
					work.Save(dao);
				else
					work.Update<DAO.GroupInfo>(ref dao);

				work.End();
			}

			dao.CloneTo(groupInfo);
		}

		public void DeleteChannelsGroup(GroupInfo groupInfo)
		{
			#region Validate parameters
			if (groupInfo == null)
				throw new ArgumentNullException("groupInfo");
			#endregion

			DAO.GroupInfo dao = groupInfo.ToDao();

			using (UnitOfWork work = BeginWork())
			{
				work.Delete(dao);
				work.End();
			}
		}

		public List<GroupChannelMap> GetGroupsChannelsMap()
		{
			using (IDataQuery dataQuery = OpenQuery())
			{
				return dataQuery.Open<DAO.GroupChannelMap>().List().Select(dao => dao.ToObj()).ToList();
			}
		}

		public void SaveGroupsChannelsMap(GroupChannelMap map)
		{
			#region Validate parameters
			if (map == null)
				throw new ArgumentNullException("map");
			#endregion

			DAO.GroupChannelMap dao = map.ToDao();

			using (UnitOfWork work = BeginWork())
			{
				if (dao.LINK == 0)
					work.Save(dao);
				else
					work.Update<DAO.GroupChannelMap>(ref dao);

				work.End();
			}

			dao.CloneTo(map);
		}

		public void DeleteGroupsChannelsMap(GroupChannelMap map)
		{
			#region Validate parameters
			if (map == null)
				throw new ArgumentNullException("map");
			#endregion

			DAO.GroupChannelMap dao = map.ToDao();

			using (UnitOfWork work = BeginWork())
			{
				work.Delete(dao);
				work.End();
			}
		}
	}
}
