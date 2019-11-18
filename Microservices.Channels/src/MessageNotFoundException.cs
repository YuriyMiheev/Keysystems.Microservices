using System;
using System.Runtime.Serialization;

namespace Microservices.Channels
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class MessageNotFoundException : Exception
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public MessageNotFoundException()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		public MessageNotFoundException(int msgLink)
			: base(String.Format("Сообщение #{0} не найдено.", msgLink))
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public MessageNotFoundException(string message)
			: base(message)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public MessageNotFoundException(string message, Exception inner)
			: base(message, inner)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected MessageNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion

	}
}
