using System;

using DAO = Microservices.Bus.Data.DAO;

namespace Microservices.Bus.Channels
{
	public static class GroupInfoExtensions
	{
		public static DAO.GroupInfo ToDao(this GroupInfo obj)
		{
			var dao = new DAO.GroupInfo();
			dao.LINK = obj.LINK;
			dao.Name = (String.IsNullOrEmpty(obj.Name) ? null : obj.Name);
			dao.Image = (String.IsNullOrEmpty(obj.Image) ? null : obj.Image);

			return dao;
		}

		public static GroupInfo ToObj(this DAO.GroupInfo dao)
		{
			if (dao == null)
				return null;

			var obj = new GroupInfo();
			dao.CloneTo(obj);

			return obj;
		}

		public static void CloneTo(this DAO.GroupInfo dao, GroupInfo obj)
		{
			#region Validate parameters
			if (dao == null)
				throw new ArgumentNullException("dao");

			if (obj == null)
				throw new ArgumentNullException("obj");
			#endregion

			obj.LINK = dao.LINK;
			obj.Name = dao.Name;
			obj.Image = dao.Image;
		}
	}
}
