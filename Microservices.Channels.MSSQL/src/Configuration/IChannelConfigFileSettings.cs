using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microservices.Channels.Configuration;

namespace Microservices.Channels.MSSQL.Configuration
{
	public interface IChannelConfigFileSettings : IXmlConfigFileSettings
	{
		string Name { get; }

		string VirtAddress { get; }

		string RealAddress { get; }

		string SID { get; }

		TimeSpan Timeout { get; }

		string PasswordIn { get; }

		string PasswordOut { get; }
	}
}
