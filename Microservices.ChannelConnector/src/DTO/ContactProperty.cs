using System;

namespace Keysystems.RemoteMessaging.DTO
{
	/// <summary>
	/// Дополнительное свойство контакта.
	/// </summary>
	[Serializable]
	public class ContactProperty
	{

		#region Properties
		private int link;
		/// <summary>
		/// {Get,Set} Внутренний ID.
		/// </summary>
		public int LINK
		{
			get { return link; }
			set { link = value; }
		}

		private int? contactLINK;
		/// <summary>
		/// {Get,Set} Ссылка на контакт.
		/// </summary>
		public int? ContactLINK
		{
			get { return contactLINK; }
			set { contactLINK = value; }
		}

		private string name;
		/// <summary>
		/// {Get,Set} Имя.
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string value;
		/// <summary>
		/// {Get,Set} Значение.
		/// </summary>
		public string Value
		{
			get { return value; }
			set { this.value = value; }
		}

		private string type;
		/// <summary>
		/// {Get,Set} Тип.
		/// </summary>
		public string Type
		{
			get { return type; }
			set { type = value; }
		}

		private string format;
		/// <summary>
		/// {Get,Set} Формат.
		/// </summary>
		public string Format
		{
			get { return format; }
			set { format = value; }
		}

		private string comment;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		public string Comment
		{
			get { return comment; }
			set { comment = value; }
		}
		#endregion

	}
}
