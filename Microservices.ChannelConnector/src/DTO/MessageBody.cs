using System;
using System.Xml.Serialization;

namespace Keysystems.RemoteMessaging.DTO
{
	/// <summary>
	/// Тело сообщения.
	/// </summary>
	[Serializable]
	[XmlRoot(ElementName = "Body", Namespace = XmlNamespaces.Rms)]
	public class MessageBody
	{

		#region Properties
		private int? messageLINK;
		/// <summary>
		/// {Get} ID сообщения.
		/// </summary>
		[XmlIgnore]
		public int? MessageLINK
		{
			get { return messageLINK; }
			set { messageLINK = value; }
		}

		private string name;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		[XmlIgnore]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string type;
		/// <summary>
		/// {Get,Set}
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
