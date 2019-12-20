using System.Collections.Generic;

using Microservices.Configuration;

namespace Microservices.Bus.Channels
{
	public static class ChannelPropertyExtensions
	{
		public static IDictionary<string, AppConfigSetting> ToAppSettings(this IDictionary<string, ChannelProperty> properties)
		{
		}

		public static IDictionary<string, ChannelProperty> ToChannelProperties(this IDictionary<string, AppConfigSetting> appSettings)
		{
		}
	}
}
