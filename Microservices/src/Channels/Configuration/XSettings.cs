using System.Collections.Generic;

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
		public int BufferSize
		{
			get => Parser.ParseInt(GetValue("X.BufferSize"), 4096) * 1024;
		}

		/// <summary>
		/// 
		/// </summary>
		public bool LogHttpRequest
		{
			get => Parser.ParseBool(GetValue("X.Log.HttpRequest"), false);
		}

		/// <summary>
		/// 
		/// </summary>
		public bool DebugEnabled
		{
			get => Parser.ParseBool(GetValue("X.Debug.Enabled"), false);
		}

		public int ProcessId
		{
			get => Parser.ParseInt(GetValue("X.ProcessId"), 0);
		}

		public bool ShowWindow
		{
			get => Parser.ParseBool(GetValue("X.ShowWindow"), true);
		}
		#endregion

	}
}
