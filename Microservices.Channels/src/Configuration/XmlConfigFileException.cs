using System;
using System.Runtime.Serialization;

namespace Microservices.Common.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class XmlConfigFileException : Exception
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public XmlConfigFileException()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public XmlConfigFileException(string message)
			: base(message)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public XmlConfigFileException(string message, Exception inner)
			: base(message, inner)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected XmlConfigFileException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion

	}
}
