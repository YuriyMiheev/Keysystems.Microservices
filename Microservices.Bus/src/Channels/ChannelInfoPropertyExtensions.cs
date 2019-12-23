using System;

using AutoMapper;

using Microservices.Configuration;

using DAO = Microservices.Bus.Data.DAO;

namespace Microservices.Bus.Channels
{
	public static class ChannelInfoPropertyExtensions
	{
		private readonly static IMapper mapper;

		static ChannelInfoPropertyExtensions()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<ChannelInfoProperty, AppConfigSetting>();
				cfg.CreateMap<AppConfigSetting, ChannelInfoProperty>();
			});
			mapper = config.CreateMapper();
		}

		public static AppConfigSetting ToAppConfigSetting(this ChannelInfoProperty property)
		{
			return mapper.Map<ChannelInfoProperty, AppConfigSetting>(property);
		}

		public static ChannelInfoProperty ToChannelInfoProperty(this AppConfigSetting appSetting)
		{
			return mapper.Map<AppConfigSetting, ChannelInfoProperty>(appSetting);
		}

		public static DAO.ChannelInfoProperty ToDao(this ChannelInfoProperty obj, DAO.ChannelInfo channel)
		{
			if (obj == null)
				return null;

			var dao = new DAO.ChannelInfoProperty();
			dao.Channel = channel;
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

		public static ChannelInfoProperty ToObj(this DAO.ChannelInfoProperty dao)
		{
			if (dao == null)
				return null;

			var obj = new ChannelInfoProperty();
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
