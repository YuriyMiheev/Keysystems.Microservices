using Microservices.Configuration;

namespace Microservices.Channels.Configuration
{
	public static class AppSettingsConfigExtensions
	{
		public static XSettings XSettings(this IAppSettingsConfig appConfig)
		{
			return new XSettings(appConfig.GetAppSettings());
		}
	}
}
