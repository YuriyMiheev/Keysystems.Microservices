using System;
using System.Runtime.Serialization;

namespace Microservices.Channels
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class MessageContentNotFoundException : Exception
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public MessageContentNotFoundException()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentLink"></param>
		public MessageContentNotFoundException(int contentLink)
			: base(String.Format("Содержимое #{0} сообщения не найдено.", contentLink))
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		/// <param name="contentLink"></param>
		/// <param name="msgLink"></param>
		public MessageContentNotFoundException(int msgLink, int contentLink)
			: base(String.Format("Содержимое #{0} сообщения #{1} не найдено.", contentLink, msgLink))
		{ }

		///// <summary>
		///// 
		///// </summary>
		///// <param name="contentLink"></param>
		///// <param name="channelInfo"></param>
		//public MessageContentNotFoundException(int contentLink, ChannelInfo channelInfo)
		//	: base(String.Format("Содержимое #{0} сообщения канала {1} не найдено.", contentLink, channelInfo))
		//{ }

		///// <summary>
		///// 
		///// </summary>
		///// <param name="msgLink"></param>
		///// <param name="contentLink"></param>
		///// <param name="channelInfo"></param>
		//public MessageContentNotFoundException(int msgLink, int contentLink, ChannelInfo channelInfo)
		//	: base(String.Format("Содержимое #{0} сообщения #{1} канала {2} не найдено.", contentLink, msgLink, channelInfo))
		//{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		/// <param name="contentName"></param>
		/// <param name="msgLink"></param>
		public MessageContentNotFoundException(int msgLink, string contentName)
			: base(String.Format("Содержимое \"{0}\" сообщения #{1} не найдено.", contentName, msgLink))
		{ }

		///// <summary>
		///// 
		///// </summary>
		///// <param name="msgLink"></param>
		///// <param name="contentName"></param>
		///// <param name="channelInfo"></param>
		//public MessageContentNotFoundException(int msgLink, string contentName, ChannelInfo channelInfo)
		//	: base(String.Format("Содержимое \"{0}\" сообщения #{1} канала {2} не найдено.", contentName, msgLink, channelInfo))
		//{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public MessageContentNotFoundException(string message)
			: base(message)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public MessageContentNotFoundException(string message, Exception inner)
			: base(message, inner)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected MessageContentNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion

	}
}
