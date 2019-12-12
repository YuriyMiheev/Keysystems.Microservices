using System;
using System.Xml.Serialization;

namespace Keysystems.RemoteMessaging.DTO
{
	/// <summary>
	/// Содержимое сообщения.
	/// </summary>
	[Serializable]
	[XmlRoot(Namespace = XmlNamespaces.Rms, ElementName = "Content")]
	public class MessageContent
	{

		#region Properties
		private int? link;
		/// <summary>
		/// {Get,Set} Внутренний ID.
		/// </summary>
		[XmlElement(ElementName = "LINK", IsNullable = true)]
		public int? LINK
		{
			get { return link; }
			set { link = value; }
		}

		private int? messageLINK;
		/// <summary>
		/// {Get,Set} Ссылка на сообщение.
		/// </summary>
		[XmlIgnore]
		public int? MessageLINK
		{
			get { return messageLINK; }
			set { messageLINK = value; }
		}

		private string name;
		/// <summary>
		/// {Get,Set} Имя.
		/// </summary>
		[XmlElement(ElementName = "Name")]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string type;
		/// <summary>
		/// {Get,Set} Тип.
		/// </summary>
		[XmlIgnore]
		public string Type
		{
			get { return type; }
			set { type = value; }
		}

		private int? length;
		/// <summary>
		/// {Get,Set} Размер содержимого.
		/// </summary>
		[XmlIgnore]
		public int? Length
		{
			get { return length; }
			set { length = value; }
		}

		private int? fileSize;
		/// <summary>
		/// {Get,Set} Размер файла.
		/// </summary>
		[XmlIgnore]
		public int? FileSize
		{
			get { return fileSize; }
			set { fileSize = value; }
		}

		private string comment;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		[XmlIgnore]
		public string Comment
		{
			get { return comment; }
			set { comment = value; }
		}

		private string value;
		/// <summary>
		/// {Get,Set} Содержимое.
		/// </summary>
		[XmlElement(ElementName = "Value", IsNullable = true)]
		public string Value
		{
			get { return value; }
			set { this.value = value; }
		}
		#endregion

	}
}
