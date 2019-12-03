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
		public string AccessKey
		{
			get => Parser.ParseString(PropertyValue("X.AccessKey"), "");
		}

		public int BufferSize
		{
			get => Parser.ParseInt(PropertyValue("X.BufferSize"), 4096) * 1024;
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public bool LogHttpRequest
		{
			get => Parser.ParseBool(PropertyValue("X.Log.HttpRequest"), false);
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public bool DebugEnabled
		{
			get => Parser.ParseBool(PropertyValue("X.Debug.Enabled"), false);
		}
		#endregion

	}
}
