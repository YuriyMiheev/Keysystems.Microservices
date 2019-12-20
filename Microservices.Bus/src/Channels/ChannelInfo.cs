using System.Collections.Generic;
using System.Linq;

using Microservices.Channels.Configuration;
using Microservices.Configuration;

namespace Microservices.Bus.Channels
{
	public class ChannelInfo : MainSettings
	{
		private readonly IDictionary<string, ChannelProperty> _properties;


		public ChannelInfo(IDictionary<string, AppConfigSetting> appSettings)
			: base(appSettings)
		{
			_properties = new Dictionary<string, ChannelProperty>(appSettings.Where(p => !p.Key.StartsWith(TAG_PREFIX)).ToChannelProperties());
		}


		public IDictionary<string, ChannelProperty> Properties => _properties;

		public int LINK { get; set; }

		public bool Enabled { get; set; }

	}
}
