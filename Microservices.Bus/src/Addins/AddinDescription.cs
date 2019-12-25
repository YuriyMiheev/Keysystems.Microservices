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
		private readonly IDictionary<string, AppConfigSetting> _mainSettings;


		public AddinDescription(string addinPath, string descriptionFile, IDictionary<string, AppConfigSetting> addinSettings )
		{
			this.AddinPath = addinPath;
			this.DescriptionFile = descriptionFile;

			_mainSettings = new Dictionary<string, AppConfigSetting>(addinSettings.Where(p => p.Key.StartsWith(".")));
			var propSettings = addinSettings.Where(p => !p.Key.StartsWith("."));

			this.Properties = new Dictionary<string, AddinDescriptionProperty>();
			foreach (KeyValuePair<string, AppConfigSetting> kvp in propSettings)
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

		public string AddinPath { get; }

		public string DescriptionFile { get; }


		private string GetValue(string propName)
		{
			if (_mainSettings.ContainsKey(propName))
				return _mainSettings[propName].Value;

			return null;
		}
	}
}
