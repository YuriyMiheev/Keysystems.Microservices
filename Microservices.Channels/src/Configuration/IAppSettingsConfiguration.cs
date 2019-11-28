using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Channels.Configuration
{
	public interface IAppSettingsConfiguration
	{
		IDictionary<string, ConfigFileSetting> GetAppSettings();

		void SetAppSettings(IDictionary<string, string> settings);

		void SaveAppSettings();
	}
}
