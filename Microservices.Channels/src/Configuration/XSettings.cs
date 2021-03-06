﻿using System.Collections.Generic;

using Microservices.Configuration;

namespace Microservices.Channels.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public class XSettings : AppSettingsBase
	{
		public const string TAG_PREFIX = "X.";


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configuration"></param>
		public XSettings(IDictionary<string, AppConfigSetting> appSettings)
				: base(TAG_PREFIX, appSettings)
		{ }
		#endregion


		#region Properties
		//public string AccessKey
		//{
		//	get => Parser.ParseString(PropertyValue("X.AccessKey"), "");
		//}

		public int BufferSize
		{
			get => Parser.ParseInt(GetValue("X.BufferSize"), 4096) * 1024;
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public bool LogHttpRequest
		{
			get => Parser.ParseBool(GetValue("X.Log.HttpRequest"), false);
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public bool DebugEnabled
		{
			get => Parser.ParseBool(GetValue("X.Debug.Enabled"), false);
		}

		//public int ProcessId
		//{
		//	get => Parser.ParseInt(PropertyValue("X.ProcessId"), 0);
		//}
		#endregion

	}
}
