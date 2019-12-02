namespace Microservices.Channels.Configuration
{
	public static class XmlConfigFileConfigurationExtensions
	{
		public static InfoSettings InfoSettings(this IAppSettingsConfig appConfig)
		{
			return new InfoSettings(appConfig.GetAppSettings());
		}

		public static ChannelSettings ChannelSettings(this IAppSettingsConfig appConfig)
		{
			return new ChannelSettings(appConfig.GetAppSettings());
		}

		public static DatabaseSettings DatabaseSettings(this IAppSettingsConfig appConfig)
		{
			return new DatabaseSettings(appConfig.GetAppSettings());
		}

		public static MessageSettings MessageSettings(this IAppSettingsConfig appConfig)
		{
			return new MessageSettings(appConfig.GetAppSettings());
		}

		public static ServiceSettings ServiceSettings(this IAppSettingsConfig appConfig)
		{
			return new ServiceSettings(appConfig.GetAppSettings());
		}
	}
}
