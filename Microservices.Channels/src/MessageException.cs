using System;
using System.Runtime.Serialization;

namespace Microservices.Channels
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class MessageException : Exception
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public MessageException()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public MessageException(string message)
			: base(message)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public MessageException(string message, Exception inner)
			: base(message, inner)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected MessageException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion

	}
}
