using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microservices.Configuration;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Описание канала.
	/// </summary>
	[DebuggerDisplay("{this.Provider}")]
	public class ChannelDescription
	{
		private readonly IDictionary<string, AppConfigSetting> _addinSettings;
		private readonly List<ChannelDescriptionProperty> _properties;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_appSettings"></param>
		public ChannelDescription(IDictionary<string, AppConfigSetting> appSettings)
		{
			string prefix = ".";
			_addinSettings = new Dictionary<string, AppConfigSetting>(appSettings.Where(p => p.Key.StartsWith(prefix)));
			_properties = new List<ChannelDescriptionProperty>();
			foreach (string key in appSettings.Keys.Where(k => !k.StartsWith(prefix)))
			{
				AppConfigSetting setting = appSettings[key];
				var descProp = new ChannelDescriptionProperty()
					{
						Comment = setting.Comment,
						Format = setting.Format,
						Default = setting.Default,
						Name = setting.Name,
						ReadOnly = setting.ReadOnly,
						Secret = setting.Secret,
						Type = setting.Type,
						Value = setting.Value
					};
				_properties.Add(descProp);
			}
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Провайдер.
		/// </summary>
		public string Provider
		{
			get { return Parser.ParseString(PropertyValue(".Provider"), ""); }
		}

		/// <summary>
		/// {Get} Имя типа и сборки, реализующих канал.
		/// </summary>
		public string Type
		{
			get { return Parser.ParseString(PropertyValue(".Type"), ""); }
		}

		/// <summary>
		/// {Get} Версия.
		/// </summary>
		public string Version
		{
			get { return Parser.ParseString(PropertyValue(".Version"), ""); }
		}

		/// <summary>
		/// {Get} Физический адрес.
		/// </summary>
		public string RealAddress
		{
			get { return Parser.ParseString(PropertyValue(".RealAddress"), ""); }
		}

		/// <summary>
		/// {Get} Таймаут (сек).
		/// </summary>
		public TimeSpan Timeout
		{
			get { return Parser.ParseTime(PropertyValue(".Timeout"), TimeSpan.FromSeconds(30)).Value; }
		}

		/// <summary>
		/// {Get} Комментарий.
		/// </summary>
		public string Comment
		{
			get { return Parser.ParseString(PropertyValue(".Comment"), ""); }
		}

		/// <summary>
		/// {Get} Может обновлять список контактов.
		/// </summary>
		public bool CanSyncContacts
		{
			get { return Parser.ParseBool(PropertyValue(".CanSyncContacts"), false); }
		}

		/// <summary>
		/// {Get} Имя файла иконки.
		/// </summary>
		public string Icon
		{
			get { return Parser.ParseString(PropertyValue(".Icon"), "favicon.png"); }
		}

		/// <summary>
		/// {Get,Set} Путь к исполняемым файлам.
		/// </summary>
		public string BinPath { get; set; }

		/// <summary>
		/// {Get} Поддержка множества экземпляров.
		/// </summary>
		public bool AllowMultipleInstances
		{
			get { return Parser.ParseBool(PropertyValue(".AllowMultipleInstances"), false); }
		}

		/// <summary>
		/// {Get} Дополнительные свойства канала.
		/// </summary>
		public ChannelDescriptionProperty[] Properties
		{
			get { return _properties.ToArray(); }
		}
		#endregion


		private string PropertyValue(string propName)
		{
			if (_addinSettings.ContainsKey(propName))
				return _addinSettings[propName].Value;

			return null;
		}

	}
}
