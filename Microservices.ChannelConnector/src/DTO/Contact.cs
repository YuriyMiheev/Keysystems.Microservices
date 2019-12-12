using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Microservices.Channels.DTO
{
	/// <summary>
	/// Информация о контакте.
	/// </summary>
	[Serializable]
	[XmlRoot(ElementName = "Contact")]
	public class Contact
	{

		#region Properties
		private int link;
		/// <summary>
		/// {Get,Set} Внутренний ID.
		/// </summary>
		[XmlElement(ElementName = "LINK")]
		public int LINK
		{
			get { return link; }
			set { link = value; }
		}

		private int contactID;
		/// <summary>
		/// {Get,Set} ID контакта.
		/// </summary>
		[XmlElement(ElementName = "ContactID")]
		public int ContactID
		{
			get { return contactID; }
			set { contactID = value; }
		}

		private string type;
		/// <summary>
		/// {Get,Set} Тип.
		/// </summary>
		[XmlElement(ElementName = "Type", IsNullable = true)]
		public string Type
		{
			get { return type; }
			set { type = value; }
		}

		private string name;
		/// <summary>
		/// {Get,Set} Отображаемое имя.
		/// </summary>
		[XmlElement(ElementName = "Name", IsNullable = true)]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string address;
		/// <summary>
		/// {Get,Set} Виртуальный адрес.
		/// </summary>
		[XmlElement(ElementName = "Address", IsNullable = true)]
		public string Address
		{
			get { return address; }
			set { address = value; }
		}

		private bool enabled;
		/// <summary>
		/// {Get,Set} Включен/Выключен.
		/// </summary>
		[XmlElement(ElementName = "Enabled")]
		public bool Enabled
		{
			get { return enabled; }
			set { enabled = value; }
		}

		private string comment;
		/// <summary>
		/// {Get,Set} Комментарий.
		/// </summary>
		[XmlElement(ElementName = "Comment", IsNullable = true)]
		public string Comment
		{
			get { return comment; }
			set { comment = value; }
		}

		private bool isService;
		/// <summary>
		/// {Get,Set} Контакт сервиса.
		/// </summary>
		[XmlElement(ElementName = "IsService")]
		public bool IsService
		{
			get { return isService; }
			set { isService = value; }
		}

		private bool isMyself;
		/// <summary>
		/// {Get,Set} Собственный контакт данного клиента/канала.
		/// </summary>
		[XmlElement(ElementName = "IsMyself")]
		public bool IsMyself
		{
			get { return isMyself; }
			set { isMyself = value; }
		}

		private bool isRemote;
		/// <summary>
		/// {Get,Set} 
		/// </summary>
		[XmlElement(ElementName = "IsRemote")]
		public bool IsRemote
		{
			get { return isRemote; }
			set { isRemote = value; }
		}

		private bool? online;
		/// <summary>
		/// {Get,Set} 
		/// </summary>
		[XmlElement(ElementName = "Online", IsNullable = true)]
		public bool? Online
		{
			get { return online; }
			set { online = value; }
		}

		private List<ContactProperty> properties;
		/// <summary>
		/// {Get,Set} Дополнительные св-ва.
		/// </summary>
		[XmlArray(ElementName = "Properties")]
		public List<ContactProperty> Properties
		{
			get { return properties = (properties ?? new List<ContactProperty>()); }
			set { properties = value; }
		}
		#endregion

	}
}
