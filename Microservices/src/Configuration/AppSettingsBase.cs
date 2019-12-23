using System.Collections.Generic;
using System.Linq;

namespace Microservices.Configuration
{
	/// <summary>
	/// Настройки.
	/// </summary>
	public abstract class AppSettingsBase
	{
		private IDictionary<string, AppConfigSetting> _appSettings;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="appSettings"></param>
		protected AppSettingsBase(string prefix, IDictionary<string, AppConfigSetting> appSettings)
		{
			_appSettings = new Dictionary<string, AppConfigSetting>(appSettings.Where(p => p.Key.StartsWith(prefix)));
		}
		#endregion


		#region Methods
		//public void Update(IDictionary<string, string> settings)
		//{
		//	foreach (string key in settings.Keys)
		//	{
		//		if (_settings.ContainsKey(key))
		//			_settings[key].Value = settings[key];
		//	}
		//}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propName"></param>
		/// <returns></returns>
		public string GetValue(string propName)
		{
			if (_appSettings.ContainsKey(propName) )
				return _appSettings[propName].Value;

			return null;
		}

		public void SetValue(string propName, string value)
		{
			_appSettings[propName].Value = value;
		}
		#endregion

	}
}
