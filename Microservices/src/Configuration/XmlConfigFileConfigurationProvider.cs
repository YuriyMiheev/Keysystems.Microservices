using System.Collections.Generic;
using System.IO;
using System.Xml;

using Microsoft.Extensions.Configuration;

namespace Microservices.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public class XmlConfigFileConfigurationProvider : FileConfigurationProvider, IAppSettingsConfig, IConnectionStringsConfig
	{
		private IDictionary<string, AppConfigSetting> _appSettings;
		private IDictionary<string, ConnectionStringSetting> _connSettings;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configFile"></param>
		public XmlConfigFileConfigurationProvider(string configFile)
			: base(new XmlConfigFileConfigurationSource(configFile))
		{
			_appSettings = new Dictionary<string, AppConfigSetting>();
			_connSettings = new Dictionary<string, ConnectionStringSetting>();
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public string ConfigFile => base.Source.Path;
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		public override void Load()
		{
			using (FileStream stream = File.OpenRead(base.Source.Path))
			{
				Load(stream);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		public override void Load(Stream stream)
		{
			var xmldoc = new XmlDocument();
			xmldoc.Load(stream);

			XmlNodeList nodes = xmldoc.SelectNodes("configuration/connectionStrings/add");
			foreach (XmlNode node in nodes)
			{
				string name = node.Attributes["name"].Value;
				string provider = node.Attributes["providerName"].Value;
				string connString = node.Attributes["connectionString"].Value;

				var setting = new ConnectionStringSetting(name, connString) { Provider = provider };
				_connSettings.Add(setting.Name, setting);
			}


			nodes = xmldoc.SelectNodes("configuration/appSettings/add");
			foreach (XmlNode node in nodes)
			{
				string key = node.Attributes["key"].Value;
				string value = node.Attributes["value"].Value;
				string type = node.Attributes["type"]?.Value;
				string format = node.Attributes["format"]?.Value;
				string defaultValue = node.Attributes["default"]?.Value;
				string comment = node.Attributes["comment"]?.Value;
				bool readOnly = Parser.ParseBool(node.Attributes["readonly"]?.Value, false);
				bool secret = Parser.ParseBool(node.Attributes["secret"]?.Value, false);

				var setting = new AppConfigSetting(key, value) { Type = type, Format = format, DefaultValue = defaultValue, Comment = comment, ReadOnly = readOnly, Secret = secret };
				_appSettings.Add(setting.Name, setting);

				//this.Data.Add(key, value);
			}
		}

		public IDictionary<string, AppConfigSetting> GetAppSettings()
		{
			return _appSettings;
		}

		public void SetAppSettings(IDictionary<string, string> settings)
		{
			foreach (string key in settings.Keys)
			{
				if (_appSettings.ContainsKey(key))
					_appSettings[key].Value = settings[key];
			}
		}

		public void SaveAppSettings()
		{
		}

		public IDictionary<string, ConnectionStringSetting> GetConnectionStrings()
		{
			return _connSettings;
		}
		#endregion

	}
}
