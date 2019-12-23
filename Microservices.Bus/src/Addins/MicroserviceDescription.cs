using System.Collections.Generic;
using System.Linq;

using Microservices.Channels.Configuration;
using Microservices.Configuration;

namespace Microservices.Bus.Addins
{
	public class MicroserviceDescription : IMainChannelSettings //AppSettingsBase
	{
		private readonly IDictionary<string, AppConfigSetting> _appSettings;
		private readonly IDictionary<string, MicroserviceDescriptionProperty> _properties;


		public MicroserviceDescription(IDictionary<string, AppConfigSetting> appSettings)
		{
			_appSettings = new Dictionary<string, AppConfigSetting>(appSettings.Where(p => p.Key.StartsWith(".")));
			_properties = new Dictionary<string, MicroserviceDescriptionProperty>();
			var otherSettings = appSettings.Where(p => !p.Key.StartsWith("."));
			foreach (KeyValuePair<string, AppConfigSetting> kvp in otherSettings)
			{
				MicroserviceDescriptionProperty prop = kvp.Value.ToMicroserviceDescriptionProperty();
				_properties.Add(prop.Name, prop);
			}
		}


		public IDictionary<string, MicroserviceDescriptionProperty> Properties => _properties;

		/// <summary>
		/// {Get} Провайдер.
		/// </summary>
		public string Provider
		{
			get => Parser.ParseString(GetValue(".Provider"), "");
		}

		/// <summary>
		/// {Get} Имя типа и сборки, реализующих канал.
		/// </summary>
		public string Type
		{
			get => Parser.ParseString(GetValue(".Type"), "");
		}

		/// <summary>
		/// {Get} Версия.
		/// </summary>
		public string Version
		{
			get => Parser.ParseString(GetValue(".Version"), "");
		}

		/// <summary>
		/// {Get} Комментарий.
		/// </summary>
		public string Comment
		{
			get => Parser.ParseString(GetValue(".Comment"), "");
		}

		/// <summary>
		/// {Get} Имя файла иконки.
		/// </summary>
		public string Icon
		{
			get => Parser.ParseString(GetValue(".Icon"), "favicon.png");
		}

		/// <summary>
		/// {Get} Может обновлять список контактов.
		/// </summary>
		public bool CanSyncContacts
		{
			get { return Parser.ParseBool(GetValue(".CanSyncContacts"), false); }
		}

		/// <summary>
		/// {Get} Поддержка множества экземпляров.
		/// </summary>
		public bool AllowMultipleInstances
		{
			get { return Parser.ParseBool(GetValue(".AllowMultipleInstances"), false); }
		}

		public string Name
		{
			get { return Parser.ParseString(GetValue(".Name"), ""); }
		}

		public string VirtAddress
		{
			get { return Parser.ParseString(GetValue(".VirtAddress"), ""); }
		}

		public string RealAddress
		{
			get { return Parser.ParseString(GetValue(".RealAddress"), ""); }
		}

		public string SID
		{
			get { return Parser.ParseString(GetValue(".SID"), ""); }
		}

		public int Timeout
		{
			get { return Parser.ParseInt(GetValue(".Timeout"), 30); }
		}

		public string PasswordIn
		{
			get { return Parser.ParseString(GetValue(".PasswordIn"), ""); }
		}

		public string PasswordOut
		{
			get { return Parser.ParseString(GetValue(".PasswordOut"), ""); }
		}

		public string BinPath { get; set; }


		private string GetValue(string propName)
		{
			if (_appSettings.ContainsKey(propName))
				return _appSettings[propName].Value;

			return null;
		}
	}
}
