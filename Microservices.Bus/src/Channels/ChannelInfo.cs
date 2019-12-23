using System.Collections.Generic;

using Microservices.Channels.Configuration;

namespace Microservices.Bus.Channels
{
	public class ChannelInfo : IMainChannelSettings
	{
		private readonly IDictionary<string, ChannelInfoProperty> _properties;


		public ChannelInfo()
		{
			_properties = new Dictionary<string, ChannelInfoProperty>();
		}

		//public ChannelInfo(IDictionary<string, AppConfigSetting> appSettings)
		//	: base(appSettings)
		//{
		//	_properties = new Dictionary<string, ChannelInfoProperty>();
		//	var otherSettings = appSettings.Where(p => !p.Key.StartsWith(TAG_PREFIX));
		//	foreach (KeyValuePair<string, AppConfigSetting> kvp in otherSettings)
		//	{
		//		ChannelInfoProperty prop = kvp.Value.ToChannelInfoProperty();
		//		prop.ChannelLINK = this.LINK;

		//		_properties.Add(prop.Name, prop);
		//	}
		//}


		public IDictionary<string, ChannelInfoProperty> Properties => _properties;

		public int LINK { get; set; }

		public bool Enabled { get; set; }

		public string Provider { get; set; }

		public string Type { get; set; }

		//public string Version { get; set; }

		public string Comment { get; set; }

		//public string Icon { get; set; }

		//public bool CanSyncContacts { get; set; }

		//public bool AllowMultipleInstances { get; set; }

		public string Name { get; set; }

		public string VirtAddress { get; set; }

		public string RealAddress { get; set; }

		public string SID { get; set; }

		public int Timeout { get; set; }

		public string PasswordIn { get; set; }

		public string PasswordOut { get; set; }
	}
}
