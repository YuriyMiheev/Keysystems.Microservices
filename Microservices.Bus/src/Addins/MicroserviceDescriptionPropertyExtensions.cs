
using System;
using AutoMapper;

using Microservices.Bus.Channels;
using Microservices.Configuration;

namespace Microservices.Bus.Addins
{
	public static class MicroserviceDescriptionPropertyExtensions
	{
		private readonly static IMapper mapper;

		static MicroserviceDescriptionPropertyExtensions()
		{
			var config = new MapperConfiguration(cfg =>
			{
				//cfg.CreateMap<MicroserviceDescriptionProperty, AppConfigSetting>();
				cfg.CreateMap<MicroserviceDescriptionProperty, ChannelInfoProperty>();
				cfg.CreateMap<AppConfigSetting, MicroserviceDescriptionProperty>();
			});
			mapper = config.CreateMapper();
		}

		//public static AppConfigSetting ToAppConfigSetting(this MicroserviceDescriptionProperty descriptionProperty)
		//{
		//	return mapper.Map<MicroserviceDescriptionProperty, AppConfigSetting>(descriptionProperty);
		//}

		public static MicroserviceDescriptionProperty ToMicroserviceDescriptionProperty(this AppConfigSetting appConfigSetting)
		{
			if (appConfigSetting == null)
				throw new ArgumentNullException(nameof(appConfigSetting));

			return mapper.Map<AppConfigSetting, MicroserviceDescriptionProperty>(appConfigSetting);
		}

		public static void CopyTo(this MicroserviceDescriptionProperty descriptionProperty, ChannelInfoProperty channelProperty)
		{
			if (descriptionProperty == null)
				throw new ArgumentNullException(nameof(descriptionProperty));

			if (channelProperty == null)
				throw new ArgumentNullException(nameof(channelProperty));

			mapper.Map<MicroserviceDescriptionProperty, ChannelInfoProperty>(descriptionProperty, channelProperty);
		}
	}
}
