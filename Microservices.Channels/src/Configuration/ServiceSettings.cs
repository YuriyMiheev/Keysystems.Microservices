using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;

//using Microservices.Channels.Configuration;
//using Microsoft.Extensions.Configuration;

namespace Microservices.Channels.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public class ServiceSettings : SettingsBase
	{
		public const string TAG_PREFIX = "X.";


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configuration"></param>
		public ServiceSettings(IDictionary<string, ConfigFileSetting> appSettings)
				: base(TAG_PREFIX, appSettings)
		{ }
		#endregion


		#region Properties
		public int BufferSize
		{
			get { return Parser.ParseInt(PropertyValue("X.BufferSize"), 4096) * 1024; }
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public bool LogHttpRequest
		{
			get { return Parser.ParseBool(PropertyValue("X.Log.HttpRequest"), false); }
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public bool DebugEnabled
		{
			get { return Parser.ParseBool(PropertyValue("X.Debug.Enabled"), false); }
		}
		#endregion


		//#region Methods
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

		////public void Save()
		////{ }
		//#endregion

	}
}
