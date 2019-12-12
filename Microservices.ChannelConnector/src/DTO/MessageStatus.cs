using System;
using System.Xml.Serialization;

namespace Microservices.ChannelConnector
{
	/// <summary>
	/// Статусы сообщений и посылок.
	/// </summary>
	[Serializable]
	//[XmlRoot(ElementName = "Status", Namespace = XmlNamespaces.Rms)]
	public class MessageStatus
	{

		#region Properties
		private string value;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		[XmlElement(ElementName = "Value", IsNullable = true)]
		//[JsonProperty("Value")]
		public string Value
		{
			get { return value; }
			set { this.value = value; }
		}

		private DateTime? date;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		[XmlElement(ElementName = "Date", IsNullable = true)]
		//[JsonProperty("Date")]
		public DateTime? Date
		{
			get { return date; }
			set { date = value; }
		}

		private string info;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		[XmlElement(ElementName = "Info", IsNullable = true)]
		//[JsonProperty("Info")]
		public string Info
		{
			get { return info; }
			set { info = value; }
		}

		private int? code;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		[XmlElement(ElementName = "Code", IsNullable = true)]
		//[JsonProperty("Code")]
		public int? Code
		{
			get { return code; }
			set { code = value; }
		}
		#endregion

	}
}
