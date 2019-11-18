using System;

namespace Microservices.Channels
{
	/// <summary>
	/// Тело сообщения.
	/// </summary>
	public class MessageBodyInfo //: MarshalByRefObject
	{

		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public MessageBodyInfo()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		public MessageBodyInfo(int msgLink)
		{
			this.MessageLINK = msgLink;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get} ID сообщения.
		/// </summary>
		public int MessageLINK { get; set; }

		/// <summary>
		/// {Get,Set} Имя.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// {Get,Set} Тип.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// {Get,Set} Фактический размер.
		/// </summary>
		public int? Length { get; set; }

		/// <summary>
		/// {Get,Set} Истинный размер.
		/// </summary>
		public int? FileSize { get; set; }
		#endregion


		#region Methods
		///// <summary>
		///// 
		///// </summary>
		///// <param name="obj"></param>
		///// <returns></returns>
		//public override bool Equals(object obj)
		//{
		//	if ( obj == null )
		//		throw new ArgumentNullException("obj");

		//	MessageBodyInfo bodyInfo = obj as MessageBodyInfo;
		//	if ( bodyInfo == null )
		//		throw new ArgumentException("obj");

		//	return (this.MessageLINK == bodyInfo.MessageLINK) && (this.Name == bodyInfo.Name);
		//}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("#{0} ({1})", this.MessageLINK, this.Name);
		}
		#endregion


		//#region MarshalByRefObject
		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//public override object InitializeLifetimeService()
		//{
		//	return null;
		//}
		//#endregion

	}
}
