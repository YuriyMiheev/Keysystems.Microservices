using System;
using System.Runtime.Serialization;

namespace Microservices.Channels
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class MessageBodyNotFoundException : Exception
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public MessageBodyNotFoundException()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		public MessageBodyNotFoundException(int msgLink)
			: base(String.Format("Тело cообщения #{0} не найдено.", msgLink))
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public MessageBodyNotFoundException(string message)
			: base(message)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public MessageBodyNotFoundException(string message, Exception inner)
			: base(message, inner)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected MessageBodyNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion

	}
}
