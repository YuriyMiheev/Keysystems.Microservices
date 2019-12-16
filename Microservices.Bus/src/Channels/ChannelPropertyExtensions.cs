using System;
using System.Collections.Generic;
using System.Text;

using DAO = Microservices.Bus.Data.DAO;

namespace Microservices.Bus.Channels
{
	public static class ChannelPropertyExtensions
	{
		public static DAO.ChannelProperty ToDao(this ChannelProperty obj, DAO.ChannelInfo channel)
		{
			if (obj == null)
				return null;

			var dao = new DAO.ChannelProperty();
			dao.Channel = channel;
			//dao.ChannelLINK = obj.ChannelLINK;
			dao.Comment = (String.IsNullOrEmpty(obj.Comment) ? null : obj.Comment);
			dao.DefaultValue = obj.DefaultValue;
			dao.Format = (String.IsNullOrEmpty(obj.Format) ? null : obj.Format);
			dao.LINK = obj.LINK;
			dao.Name = obj.Name;
			dao.ReadOnly = (obj.ReadOnly == false ? new Nullable<bool>() : obj.ReadOnly);
			dao.Secret = (obj.Secret == false ? new Nullable<bool>() : obj.Secret);
			dao.Type = (String.IsNullOrEmpty(obj.Type) ? null : obj.Type);
			dao.Value = obj.Value;

			return dao;
		}

		public static ChannelProperty ToObj(this DAO.ChannelProperty dao)
		{
			if (dao == null)
				return null;

			var obj = new ChannelProperty();
			obj.ChannelLINK = dao.Channel.LINK;
			obj.Comment = dao.Comment;
			obj.DefaultValue = dao.DefaultValue;
			obj.Format = dao.Format;
			obj.LINK = dao.LINK;
			obj.Name = dao.Name;
			obj.ReadOnly = (dao.ReadOnly == null ? false : dao.ReadOnly.Value);
			obj.Secret = (dao.Secret == null ? false : dao.Secret.Value);
			obj.Type = dao.Type;
			obj.Value = dao.Value;

			return obj;
		}
	}
}
