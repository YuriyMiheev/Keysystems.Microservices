using System.Collections.Generic;
using System.Linq;

using Microservices.Configuration;

namespace Microservices.Bus.Addins
{
	/// <summary>
	/// Описание дополнения из конфиг файла.
	/// </summary>
	public class AddinDescription : IAddinDescription
	{
		private readonly IDictionary<string, AppConfigSetting> _appSettings;


		public AddinDescription(IDictionary<string, AppConfigSetting> appSettings)
		{
			this.Properties = new Dictionary<string, AddinDescriptionProperty>();

			_appSettings = new Dictionary<string, AppConfigSetting>(appSettings.Where(p => p.Key.StartsWith(".")));
			var otherSettings = appSettings.Where(p => !p.Key.StartsWith("."));

			foreach (KeyValuePair<string, AppConfigSetting> kvp in otherSettings)
			{
				AddinDescriptionProperty prop = kvp.Value.ToDescriptionProperty();
				this.Properties.Add(prop.Name, prop);
			}
		}


		public IDictionary<string, AddinDescriptionProperty> Properties { get; }

		public string Provider
		{
			get => Parser.ParseString(GetValue(".Provider"), "");
		}

		public string Type
		{
			get => Parser.ParseString(GetValue(".Type"), "");
		}

		public string Version
		{
			get => Parser.ParseString(GetValue(".Version"), "");
		}

		public string Comment
		{
			get => Parser.ParseString(GetValue(".Comment"), "");
		}

		public string Icon
		{
			get => Parser.ParseString(GetValue(".Icon"), "favicon.png");
		}

		public bool CanSyncContacts
		{
			get => Parser.ParseBool(GetValue(".CanSyncContacts"), false);
		}

		public bool AllowMultipleInstances
		{
			get => Parser.ParseBool(GetValue(".AllowMultipleInstances"), false);
		}

		public string RealAddress
		{
			get => Parser.ParseString(GetValue(".RealAddress"), "");
		}

		public string SID
		{
			get => Parser.ParseString(GetValue(".SID"), "");
		}

		public int Timeout
		{
			get => Parser.ParseInt(GetValue(".Timeout"), 30);
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
