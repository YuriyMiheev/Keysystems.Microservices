using System;
using System.Runtime.Serialization;

namespace Microservices
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class MessagePropertyNotFoundException : Exception
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public MessagePropertyNotFoundException()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propLink"></param>
		public MessagePropertyNotFoundException(int propLink)
			: base(String.Format("Свойство #{0} сообщения не найдено.", propLink))
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propLink"></param>
		/// <param name="msgLink"></param>
		public MessagePropertyNotFoundException(int propLink, int msgLink)
			: base(String.Format("Свойство #{0} сообщения #{1} не найдено.", propLink, msgLink))
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propName"></param>
		/// <param name="msgLink"></param>
		public MessagePropertyNotFoundException(string propName, int msgLink)
			: base(String.Format("Свойство '{0}' сообщения #{1} не найдено.", propName, msgLink))
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public MessagePropertyNotFoundException(string message)
			: base(message)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public MessagePropertyNotFoundException(string message, Exception inner)
			: base(message, inner)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected MessagePropertyNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion

	}
}
