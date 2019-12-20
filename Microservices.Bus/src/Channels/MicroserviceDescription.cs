using System.Collections.Generic;
using System.Linq;

using Microservices.Channels.Configuration;
using Microservices.Configuration;

namespace Microservices.Bus.Channels
{
	public class MicroserviceDescription : MainSettings
	{
		private readonly IDictionary<string, MicroserviceDescriptionProperty> _properties;


		public MicroserviceDescription(IDictionary<string, AppConfigSetting> appSettings)
			: base(appSettings)
		{
			_properties = new Dictionary<string, MicroserviceDescriptionProperty>(appSettings.Where(p => !p.Key.StartsWith(TAG_PREFIX)));
		}


		public IDictionary<string, MicroserviceDescriptionProperty> Properties => _properties;

		public string BinPath { get; set; }
	}
}
