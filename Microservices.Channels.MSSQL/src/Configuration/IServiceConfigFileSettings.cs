using System;
using System.Collections.Generic;
using System.Net;

using Microservices.Channels.Configuration;

namespace Microservices.Channels.MSSQL.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public interface IServiceConfigFileSettings : IXmlConfigFileSettings
	{

		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		bool LogHttpRequest { get; }

		/// <summary>
		/// {Get}
		/// </summary>
		bool DebugEnabled { get; }
		#endregion

	}
}
