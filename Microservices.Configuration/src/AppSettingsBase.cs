using System.Collections.Generic;
using System.Linq;

namespace Microservices.Configuration
{
	/// <summary>
	/// Настройки.
	/// </summary>
	public abstract class AppSettingsBase
	{
		private IDictionary<string, AppConfigSetting> _settings;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="settings"></param>
		protected AppSettingsBase(string prefix, IDictionary<string, AppConfigSetting> settings)
		{
			_settings = new Dictionary<string, AppConfigSetting>(settings.Where(p => p.Key.StartsWith(prefix)));
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
		protected virtual string PropertyValue(string propName)
		{
			if (_settings.ContainsKey(propName) )
				return _settings[propName].Value;

			return null;
		}
		#endregion

	}
}
