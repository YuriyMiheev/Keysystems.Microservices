namespace Microservices.Configuration
{
	public interface IAppConfigSetting
	{
		string Name { get; set; }

		string Value { get; set; }

		string Type { get; set; }

		string Format { get; set; }

		string DefaultValue { get; set; }

		string Comment { get; set; }

		bool ReadOnly { get; set; }

		bool Secret { get; set; }
	}
}
