using System;

using DAO = Microservices.Bus.Data.DAO;

namespace Microservices.Bus.Channels
{
	public static class GroupChannelMapExtensions
	{
		public static DAO.GroupChannelMap ToDao(this GroupChannelMap obj)
		{
			if (obj == null)
				return null;

			var dao = new DAO.GroupChannelMap();
			dao.LINK = obj.LINK;
			dao.GroupLINK = obj.GroupLINK;
			dao.ChannelLINK = obj.ChannelLINK;

			return dao;
		}

		public static GroupChannelMap ToObj(this DAO.GroupChannelMap dao)
		{
			if (dao == null)
				return null;

			var obj = new GroupChannelMap();
			dao.CloneTo(obj);

			return obj;
		}

		public static void CloneTo(this DAO.GroupChannelMap dao, GroupChannelMap obj)
		{
			#region Validate parameters
			if (dao == null)
				throw new ArgumentNullException("dao");

			if (obj == null)
				throw new ArgumentNullException("obj");
			#endregion

			obj.LINK = dao.LINK;
			obj.GroupLINK = dao.GroupLINK;
			obj.ChannelLINK = dao.ChannelLINK;
		}
	}
}
