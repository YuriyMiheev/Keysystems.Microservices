using System;
using System.Linq;

using DAO = Microservices.Bus.Data.DAO;

namespace Microservices.Bus.Channels
{
	public static class ChannelInfoExtensions
	{
		//public static string Id(this ChannelInfo obj)
		//{
		//	return $"#{obj.LINK} ({obj.VirtAddress})";
		//}

		public static DAO.ChannelInfo ToDao(this ChannelInfo obj)
		{
			if (obj == null)
				return null;

			var dao = new DAO.ChannelInfo();
			//dao.AccessMode = (obj.AccessMode == AccessMode.FULL ? null : obj.AccessMode);
			dao.Comment = (String.IsNullOrEmpty(obj.Comment) ? null : obj.Comment);
			dao.Enabled = (obj.Enabled == false ? new Nullable<bool>() : obj.Enabled);
			//dao.IsolatedDomain = (obj.IsolatedDomain == false ? new Nullable<bool>() : obj.IsolatedDomain);
			dao.LINK = obj.LINK;
			dao.Name = (String.IsNullOrEmpty(obj.Name) ? null : obj.Name);
			dao.PasswordIn = (String.IsNullOrEmpty(obj.PasswordIn) ? null : obj.PasswordIn);
			dao.PasswordOut = (String.IsNullOrEmpty(obj.PasswordOut) ? null : obj.PasswordOut);
			dao.Properties = obj.Properties.Select(prop => prop.ToDao(dao)).ToList();
			dao.Provider = obj.Provider;
			dao.RealAddress = obj.RealAddress;
			dao.SID = (String.IsNullOrEmpty(obj.SID) ? null : obj.SID);
			dao.Timeout = (obj.Timeout == 0 ? new Nullable<int>() : obj.Timeout);
			dao.VirtAddress = obj.VirtAddress;

			return dao;
		}

		public static ChannelInfo ToObj(this DAO.ChannelInfo dao)
		{
			if (dao == null)
				return null;

			var obj = new ChannelInfo();
			dao.CloneTo(obj);

			return obj;
		}

		public static void CloneTo(this DAO.ChannelInfo dao, ChannelInfo obj)
		{
			#region Validate parameters
			if (dao == null)
				throw new ArgumentNullException("dao");

			if (obj == null)
				throw new ArgumentNullException("obj");
			#endregion

			//obj.AccessMode = dao.AccessMode;
			obj.Comment = dao.Comment;
			obj.Enabled = (dao.Enabled == null ? false : dao.Enabled.Value);
			obj.LINK = dao.LINK;
			obj.Name = dao.Name;
			obj.PasswordIn = dao.PasswordIn;
			obj.PasswordOut = dao.PasswordOut;
			obj.Properties = dao.Properties.Select(prop => prop.ToObj()).ToArray();
			obj.Provider = dao.Provider;
			obj.RealAddress = dao.RealAddress;
			obj.SID = dao.SID;
			obj.Timeout = (dao.Timeout == null ? 0 : dao.Timeout.Value);
			obj.VirtAddress = dao.VirtAddress;
		}

		public static ChannelSettings ChannelSettings(this ChannelInfo channelInfo)
		{
			return new ChannelSettings(channelInfo.Properties);
		}
	}
}
