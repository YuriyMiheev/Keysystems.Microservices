using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using Microsoft.Extensions.Configuration;

namespace Microservices.Channels.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public class XmlConfigFileConfigurationProvider : FileConfigurationProvider
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configFile"></param>
		public XmlConfigFileConfigurationProvider(string configFile)
			: base(new XmlConfigFileConfigurationSource(configFile))
		{
			_appSettings = new Dictionary<string, ConfigFileSetting>();
		}


		private IDictionary<string, ConfigFileSetting> _appSettings;
		/// <summary>
		/// {Get}
		/// </summary>
		public IDictionary<string, ConfigFileSetting> AppSettings
		{
			get { return _appSettings; }
		}

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

			XmlNodeList nodes = xmldoc.SelectNodes("configuration/appSettings/add");
			foreach (XmlNode node in nodes)
			{
				string key = node.Attributes["key"].Value;
				string value = node.Attributes["value"].Value;
				string type = node.Attributes["type"]?.Value;
				string format = node.Attributes["format"]?.Value;
				string defaultValue = node.Attributes["default"]?.Value;
				string comment = node.Attributes["comment"]?.Value;
				bool readOnly = Parser.ParseBool(node.Attributes["readonly"]?.Value, false);
				bool secret = Parser.ParseBool(node.Attributes["readonly"]?.Value, false);

				var setting = new ConfigFileSetting(key, value) { Type = type, Format = format, Default = defaultValue, Comment = comment, ReadOnly = readOnly, Secret = secret };
				_appSettings.Add(setting.Name, setting);

				this.Data.Add(key, value);
			}
		}
	}
}
