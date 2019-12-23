using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Configuration
{
	public class ConnectionStringSetting
	{
		public ConnectionStringSetting()
		{ }

		public ConnectionStringSetting(string name, string connectionString)
		{
			this.Name = name ?? throw new ArgumentException(nameof(name));
			this.ConnectionString = connectionString;
		}


		public string Name { get; set; }

		public string Provider { get; set; }

		public string ConnectionString { get; set; }

	}
}
