using Microservices.Channels.Configuration;

namespace Microservices.Configuration
{
	public static class AppSettingsConfigExtensions
	{
		public static MainSettings MainSettings(this IAppSettingsConfig appConfig)
		{
			return new MainSettings(appConfig.GetAppSettings());
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
	}
}
