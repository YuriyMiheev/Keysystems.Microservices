using System;
using System.Net.Mime;

namespace Microservices
{
	/// <summary>
	/// Содержимое сообщения.
	/// </summary>
	public class MessageContentInfo
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public MessageContentInfo()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		public MessageContentInfo(int msgLink)
		{
			this.MessageLINK = msgLink;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public int LINK { get;  set; }

		/// <summary>
		/// {Get} Ссылка на сообщение.
		/// </summary>
		public int MessageLINK { get;  set; }

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

		/// <summary>
		/// {Get,Set} Комментарий.
		/// </summary>
		public string Comment { get; set; }
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

		//	MessageContentInfo contentInfo = obj as MessageContentInfo;
		//	if ( contentInfo == null )
		//		throw new ArgumentException("obj");

		//	return (this.LINK == contentInfo.LINK && this.MessageLINK == contentInfo.MessageLINK) && this.Name.Equals(contentInfo.Name, StringComparison.InvariantCultureIgnoreCase);
		//}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("#{0} ({1})", this.LINK, this.Name);
		}
		#endregion

	}
}
