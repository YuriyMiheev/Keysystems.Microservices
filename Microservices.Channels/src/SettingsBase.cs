using System;
using System.Collections.Generic;
using System.Linq;

using Microservices.Channels.Configuration;

namespace Microservices.Channels
{
	/// <summary>
	/// Дополнительные настройки канала.
	/// </summary>
	public abstract class SettingsBase// : MarshalByRefObject
	{
		/// <summary>
		/// 
		/// </summary>
		protected IDictionary<string, ConfigFileSetting> _settings;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		protected SettingsBase()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="settings"></param>
		protected SettingsBase(string prefix, IDictionary<string, ConfigFileSetting> settings)
		{
			_settings = new Dictionary<string, ConfigFileSetting>(settings.Where(p => p.Key.StartsWith(prefix)));
		}
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool HasProperties()
		{
			return (_settings.Count > 0);
		}

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


		//#region MarshalByRefObject
		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//public override object InitializeLifetimeService()
		//{
		//	return null;
		//}
		//#endregion

	}
}
