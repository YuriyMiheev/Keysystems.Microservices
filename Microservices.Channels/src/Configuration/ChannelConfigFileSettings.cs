using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;

//using Microservices.Channels.Configuration;
using Microsoft.Extensions.Configuration;

namespace Microservices.Channels.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public class ChannelConfigFileSettings //: IChannelConfigFileSettings
	{
		private XmlConfigFileConfigurationProvider _configuration;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configuration"></param>
		public ChannelConfigFileSettings(XmlConfigFileConfigurationProvider configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}
		#endregion


		#region Properties
		public string Name
		{
			get { return _configuration.AppSettings[".Name"].Value; }
		}

		public string VirtAddress
		{
			get { return _configuration.AppSettings[".VirtAddress"].Value; }
		}

		public string RealAddress
		{
			get { return _configuration.AppSettings[".RealAddress"].Value; }
		}

		public string SID
		{
			get { return _configuration.AppSettings[".SID"].Value; }
		}

		public TimeSpan Timeout
		{
			get { return Parser.ParseTime(_configuration.AppSettings[".Timeout"]?.Value, TimeSpan.FromSeconds(30)).Value; }
		}

		public string PasswordIn
		{
			get { return _configuration.AppSettings[".PasswordIn"].Value; }
		}

		public string PasswordOut
		{
			get { return _configuration.AppSettings[".PasswordOut"].Value; }
		}
		#endregion


		#region Methods
		public IDictionary<string, ConfigFileSetting> GetAppSettings()
		{
			return _configuration.AppSettings;
		}

		public void SetAppSettings(IDictionary<string, string> settings)
		{
			foreach (string key in settings.Keys)
			{
				if (_configuration.AppSettings.ContainsKey(key))
					_configuration.AppSettings[key].Value = settings[key];
			}
		}

		//IDictionary<string, string> IXmlConfigFileSettings.GetConnectionStrings()
		//{
		//	return _configuration.ConnectionStrings;
		//}

		//void IXmlConfigFileSettings.SetConnectionStrings(IDictionary<string, string> settings)
		//{
		//	foreach (string key in settings.Keys)
		//	{
		//		if (_configuration.ConnectionStrings.ContainsKey(key))
		//			_configuration.ConnectionStrings[key] = settings[key];
		//	}
		//}

		//public void Save()
		//{ }
		#endregion


		//#region Helpers
		//private bool IsValueTrue(string value)
		//{
		//	value = value.ToUpper();
		//	return (value == "TRUE" || value == "ИСТИНА" || value == "YES" || value == "ДА" || value == "ON" || value == "1");
		//}
		//#endregion

	}
}
