using System.Collections.Generic;

namespace Microservices.Configuration
{
	public interface IConnectionStringsConfig
	{
		string ConfigFile { get; }

		IDictionary<string, ConnectionStringSetting> GetConnectionStrings();
	}
}
