using System.Collections.Generic;

namespace Microservices.Configuration
{
	public interface IAppSettingsConfig
	{
		string ConfigFile { get; }


		IDictionary<string, AppConfigSetting> GetAppSettings();

		void SetAppSettings(IDictionary<string, string> settings);

		void SaveAppSettings();
	}
}
