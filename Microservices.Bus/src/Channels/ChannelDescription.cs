using System.Collections.Generic;
using System.Linq;

using Microservices.Channels.Configuration;
using Microservices.Configuration;

namespace Microservices.Bus.Channels
{
	public class ChannelDescription : MainSettings
	{
		private readonly IDictionary<string, AppConfigSetting> _properties;


		public ChannelDescription(IDictionary<string, AppConfigSetting> appSettings)
			: base(appSettings)
		{
			_properties = new Dictionary<string, AppConfigSetting>(appSettings.Where(p => !p.Key.StartsWith(TAG_PREFIX)));
		}



		public string BinPath { get; set; }
	}
}
