using System;

namespace Microservices.ChannelConnector
{
	/// <summary>
	/// Дополнительное свойство сообщения.
	/// </summary>
	[Serializable]
	//[XmlRoot(Namespace = XmlNamespaces.Rms, ElementName = "Property")]
	public class MessageProperty
	{

		#region Properties
		private int? link;
		/// <summary>
		/// {Get,Set} Внутренний ID.
		/// </summary>
		//[XmlElement(ElementName = "LINK", IsNullable = true)]
		public int? LINK
		{
			get { return link; }
			set { link = value; }
		}

		private int? messageLINK;
		/// <summary>
		/// {Get,Set} Ссылка на сообщение.
		/// </summary>
		//[XmlIgnore]
		public int? MessageLINK
		{
			get { return messageLINK; }
			set { messageLINK = value; }
		}

		private string name;
		/// <summary>
		/// {Get,Set} Имя.
		/// </summary>
		//[XmlElement(ElementName = "Name")]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string value;
		/// <summary>
		/// {Get,Set} Значение.
		/// </summary>
		//[XmlElement(ElementName = "Value", IsNullable = true)]
		public string Value
		{
			get { return value; }
			set { this.value = value; }
		}

		private string type;
		/// <summary>
		/// {Get,Set} Тип.
		/// </summary>
		//[XmlElement(ElementName = "Type", IsNullable = true)]
		public string Type
		{
			get { return type; }
			set { type = value; }
		}

		private string format;
		/// <summary>
		/// {Get,Set} Формат.
		/// </summary>
		//[XmlElement(ElementName = "Format", IsNullable = true)]
		public string Format
		{
			get { return format; }
			set { format = value; }
		}

		private string comment;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		//[XmlElement(ElementName = "Comment", IsNullable = true)]
		public string Comment
		{
			get { return comment; }
			set { comment = value; }
		}
		#endregion

	}
}
