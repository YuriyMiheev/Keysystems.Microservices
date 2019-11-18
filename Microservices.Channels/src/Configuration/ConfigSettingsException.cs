using System;
using System.Runtime.Serialization;

namespace Microservices.Channels.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ConfigSettingsException : Exception
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public ConfigSettingsException()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public ConfigSettingsException(string message, string settingName)
			: base(FormatMessage(message, settingName))
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public ConfigSettingsException(string message, string settingName, Exception inner)
			: base(FormatMessage(message, settingName), inner)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ConfigSettingsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion


		private static string FormatMessage(string message, string settingName)
		{
			return String.Format("{0} Имя настройки: \"{1}\".", message, settingName);
		}
	}
}
