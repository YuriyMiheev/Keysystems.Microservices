using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Configuration
{
	public interface IConnectionStringsConfig
	{
		IDictionary<string, ConnectionStringSetting> GetConnectionStrings();
	}
}
