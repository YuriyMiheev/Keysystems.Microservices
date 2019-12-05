using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Configuration
{
	[Serializable]
	public class ConfigFileSetting
	{
		public ConfigFileSetting()
		{ }

		public ConfigFileSetting(string name, string value)
		{
			this.Name = name ?? throw new ArgumentException(nameof(name));
			this.Value = value;
		}


		public string Name { get; set; }

		public string Value { get; set; }

		public string Type { get; set; }

		public string Format { get; set; }

		public string Default { get; set; }

		public string Comment { get; set; }

		public bool ReadOnly { get; set; }

		public bool Secret { get; set; }

	}
}
