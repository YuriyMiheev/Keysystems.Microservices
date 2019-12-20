using System;

using Microservices.Configuration;

namespace Microservices.Bus.Channels
{
	public class ChannelProperty : AppConfigSetting
	{
		public ChannelProperty()
		{ }

		public ChannelProperty(string name, string value)
		{
			this.Name = name ?? throw new ArgumentException(nameof(name));
			this.Value = value;
		}


		public int LINK { get; set; }

		public int ChannelLINK { get; set; }
	}
}
