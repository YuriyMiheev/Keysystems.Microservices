using Microservices.Configuration;

namespace Microservices.Bus.Addins
{
	public class MicroserviceDescriptionProperty : IAppConfigSetting
	{
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
