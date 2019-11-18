using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;

using Microservices.Channels.Configuration;
using Microsoft.Extensions.Configuration;

namespace Microservices.Channels.MSSQL.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public class ServiceConfigFileSettings //: IServiceConfigFileSettings
	{
		private XmlConfigFileConfigurationProvider _configuration;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configuration"></param>
		public ServiceConfigFileSettings(XmlConfigFileConfigurationProvider configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}
		#endregion


		#region Properties
		public int BufferSize
		{
			get { return Parser.ParseInt(_configuration.AppSettings["BufferSize"]?.Value, 4096) * 1024; }
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public bool LogHttpRequest
		{
			get { return Parser.ParseBool(_configuration.AppSettings["Log.HttpRequest"]?.Value, false); }
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public bool DebugEnabled
		{
			get { return Parser.ParseBool(_configuration.AppSettings["Debug.Enabled"]?.Value, false); }
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

		//public IDictionary<string, string> GetConnectionStrings()
		//{
		//	return _configuration.ConnectionStrings;
		//}

		//public void SetConnectionStrings(IDictionary<string, string> settings)
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
