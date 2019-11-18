using System;
using System.Data;
using System.Runtime.Serialization;

namespace Microservices.Channels
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ConnectionException : Exception
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public ConnectionException()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public ConnectionException(string message)
			: base(message)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public ConnectionException(string message, Exception inner)
			: base(message, inner)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ConnectionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion

	}
}
