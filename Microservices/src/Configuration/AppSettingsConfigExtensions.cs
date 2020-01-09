using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		public static XSettings XSettings(this IAppSettingsConfig appConfig)
		{
			return new XSettings(appConfig.GetAppSettings());
		}


		public static string ToLoggerString(this IAppSettingsConfig appConfig)
		{
			var sb = new StringBuilder();

			IDictionary<string, AppConfigSetting> appSettings = appConfig.GetAppSettings();
			int maxLength = appSettings.Keys.Max(key => key.Length);
			foreach (string settingName in appSettings.Keys)
			{
				AppConfigSetting appSetting = appSettings[settingName];
				sb.AppendLine($"{settingName.PadRight(maxLength)} = {appSetting.Value}");
			}

			return sb.ToString();
		}
	}
}
