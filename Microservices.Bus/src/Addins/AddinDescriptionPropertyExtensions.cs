using System;

using AutoMapper;

using Microservices.Bus.Channels;
using Microservices.Configuration;

namespace Microservices.Bus.Addins
{
	public static class AddinDescriptionPropertyExtensions
	{
		private readonly static IMapper mapper;

		static AddinDescriptionPropertyExtensions()
		{
			var config = new MapperConfiguration(cfg =>
			{
				//cfg.CreateMap<MicroserviceDescriptionProperty, AppConfigSetting>();
				cfg.CreateMap<AddinDescriptionProperty, ChannelInfoProperty>();
				cfg.CreateMap<AppConfigSetting, AddinDescriptionProperty>();
			});
			mapper = config.CreateMapper();
		}

		//public static AppConfigSetting ToAppConfigSetting(this MicroserviceDescriptionProperty descriptionProperty)
		//{
		//	return mapper.Map<MicroserviceDescriptionProperty, AppConfigSetting>(descriptionProperty);
		//}

		public static AddinDescriptionProperty ToDescriptionProperty(this AppConfigSetting appConfigSetting)
		{
			if (appConfigSetting == null)
				throw new ArgumentNullException(nameof(appConfigSetting));

			return mapper.Map<AppConfigSetting, AddinDescriptionProperty>(appConfigSetting);
		}

		public static void CopyTo(this AddinDescriptionProperty descriptionProperty, ChannelInfoProperty channelProperty)
		{
			if (descriptionProperty == null)
				throw new ArgumentNullException(nameof(descriptionProperty));

			if (channelProperty == null)
				throw new ArgumentNullException(nameof(channelProperty));

			mapper.Map<AddinDescriptionProperty, ChannelInfoProperty>(descriptionProperty, channelProperty);
		}
	}
}
