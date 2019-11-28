namespace Microservices.Channels.Configuration
{
	public static class XmlConfigFileConfigurationExtensions
	{
		public static InfoSettings InfoSettings(this IAppSettingsConfiguration appConfig)
		{
			return new InfoSettings(appConfig.GetAppSettings());
		}

		public static ChannelSettings ChannelSettings(this IAppSettingsConfiguration appConfig)
		{
			return new ChannelSettings(appConfig.GetAppSettings());
		}

		public static DatabaseSettings DatabaseSettings(this IAppSettingsConfiguration appConfig)
		{
			return new DatabaseSettings(appConfig.GetAppSettings());
		}

		public static MessageSettings MessageSettings(this IAppSettingsConfiguration appConfig)
		{
			return new MessageSettings(appConfig.GetAppSettings());
		}

		public static ServiceSettings ServiceSettings(this IAppSettingsConfiguration appConfig)
		{
			return new ServiceSettings(appConfig.GetAppSettings());
		}
	}
}
