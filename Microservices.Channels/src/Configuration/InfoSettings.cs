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
	public class InfoSettings : SettingsBase
	{
		public const string TAG_PREFIX = ".";


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configuration"></param>
		public InfoSettings(IDictionary<string, ConfigFileSetting> appSettings)
			: base(TAG_PREFIX, appSettings)
		{ }
		#endregion


		#region Properties
		public string Name
		{
			get { return Parser.ParseString(PropertyValue(".Name"), ""); }
		}

		public string VirtAddress
		{
			get { return Parser.ParseString(PropertyValue(".VirtAddress"), ""); }
		}

		public string RealAddress
		{
			get { return Parser.ParseString(PropertyValue(".RealAddress"), ""); }
		}

		public string SID
		{
			get { return Parser.ParseString(PropertyValue(".SID"), ""); }
		}

		public TimeSpan Timeout
		{
			get { return Parser.ParseTime(PropertyValue(".Timeout"), TimeSpan.FromSeconds(30)).Value; }
		}

		public string PasswordIn
		{
			get { return Parser.ParseString(PropertyValue(".PasswordIn"), ""); }
		}

		public string PasswordOut
		{
			get { return Parser.ParseString(PropertyValue(".PasswordOut"), ""); }
		}
		#endregion


		#region Methods
		//public IDictionary<string, ConfigFileSetting> GetAppSettings()
		//{
		//	return _configuration.AppSettings;
		//}

		//public void SetAppSettings(IDictionary<string, string> settings)
		//{
		//	foreach (string key in settings.Keys)
		//	{
		//		if (_configuration.AppSettings.ContainsKey(key))
		//			_configuration.AppSettings[key].Value = settings[key];
		//	}
		//}

		//public void Save()
		//{ }
		#endregion

	}
}
