using System;
using System.Collections.Generic;
using System.Net;

namespace Microservices.Channels.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public interface IXmlConfigFileSettings
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IDictionary<string, ConfigFileSetting> GetAppSettings();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="settings"></param>
		void SetAppSettings(IDictionary<string, string> settings);

		//IDictionary<string, string> GetConnectionStrings();

		//void SetConnectionStrings(IDictionary<string, string> connectionStrings);

		//void Save();

	}
}
