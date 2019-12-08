using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Configuration
{
	public interface IAppSettingsConfig
	{
		IDictionary<string, AppConfigSetting> GetAppSettings();

		void SetAppSettings(IDictionary<string, string> settings);

		void SaveAppSettings();
	}
}
