using System;

namespace Microservices.Common.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public enum ServiceSettingsChangeType
	{
		/// <summary>
		/// 
		/// </summary>
		ConfigFileChanged,

		/// <summary>
		/// 
		/// </summary>
		ConfigFileCreated,

		/// <summary>
		/// 
		/// </summary>
		ConfigFileDeleted,

		/// <summary>
		/// 
		/// </summary>
		ConfigFileRenamed
	}
}
