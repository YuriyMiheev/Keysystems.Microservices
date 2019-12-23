
using Microservices.Configuration;

namespace Microservices.Bus.Channels
{
	public class ChannelInfoProperty : IAppConfigSetting
	{
		//public ChannelInfoProperty()
		//{ }

		//public ChannelInfoProperty(string name, string value)
		//{
		//	this.Name = name ?? throw new ArgumentException(nameof(name));
		//	this.Value = value;
		//}


		public int LINK { get; set; }

		public int ChannelLINK { get; set; }

		public string Name { get; set; }

		public string Value { get; set; }

		public string Type { get; set; }

		public string Format { get; set; }

		public string DefaultValue { get; set; }

		public string Comment { get; set; }

		public bool ReadOnly { get; set; }

		public bool Secret { get; set; }
	}
}
