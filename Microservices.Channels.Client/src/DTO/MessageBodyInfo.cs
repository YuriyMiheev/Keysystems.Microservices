using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Xml.Serialization;

//using Keysystems.RemoteMessaging.Lib.Collections;

namespace Microservices.Channels.Client
{
	/// <summary>
	/// Информация о теле сообщения.
	/// </summary>
	[Serializable]
	//[XmlRoot(Namespace = XmlNamespaces.Rms, ElementName = "Body")]
	public class MessageBodyInfo
	{

		#region Properties
		private int? messageLINK;
		/// <summary>
		/// {Get} ID сообщения.
		/// </summary>
		//[XmlIgnore]
		public int? MessageLINK
		{
			get { return messageLINK; }
			set { messageLINK = value; }
		}

		private string name;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		//[XmlElement(ElementName = "Name", IsNullable = true)]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string type;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		//[XmlElement(ElementName = "Type", IsNullable = true)]
		public string Type
		{
			get { return type; }
			set { type = value; }
		}

		private int? length;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		//[XmlElement(ElementName = "Length", IsNullable = true)]
		public int? Length
		{
			get { return length; }
			set { length = value; }
		}

		private int? fileSize;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		//[XmlElement(ElementName = "FileSize", IsNullable = true)]
		public int? FileSize
		{
			get { return fileSize; }
			set { fileSize = value; }
		}
		#endregion


		#region Methods
		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//public NameValueCollection ToHeaders()
		//{
		//	var headers = new NameValueCollection();
		//	headers.Add("X-RMS-MessageLINK", (this.MessageLINK == null ? null : this.MessageLINK.ToString()));
		//	headers.Add("X-RMS-MessageBodyName", (this.Name == null ? null : HttpUtility.UrlPathEncode(this.Name)));
		//	headers.Add("X-RMS-MessageBodyType", this.Type);
		//	headers.Add("X-RMS-MessageBodyLength", (this.Length == null ? null : this.Length.ToString()));
		//	headers.Add("X-RMS-MessageBodyFileSize", (this.FileSize == null ? null : this.FileSize.ToString()));
		//	return headers;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="headers"></param>
		///// <returns></returns>
		//public static MessageBodyInfo Parse(NameValueCollection headers)
		//{
		//	#region Validate parameters
		//	if ( headers == null )
		//		throw new ArgumentNullException("headers");
		//	#endregion

		//	if ( headers.HasKey("X-RMS-MessageLINK")
		//		|| headers.HasKey("X-RMS-MessageBodyName")
		//		|| headers.HasKey("X-RMS-MessageBodyType")
		//		|| headers.HasKey("X-RMS-MessageBodyLength")
		//		|| headers.HasKey("X-RMS-MessageBodyFileSize") )
		//	{
		//		var bodyInfo = new MessageBodyInfo();

		//		int msgLink;
		//		if ( Int32.TryParse(headers["X-RMS-MessageLINK"], out msgLink) )
		//			bodyInfo.MessageLINK = msgLink;

		//		string name = headers["X-RMS-MessageBodyName"];
		//		bodyInfo.Name = (name == null ? null : HttpUtility.UrlDecode(name, Encoding.UTF8));
		//		bodyInfo.Type = headers["X-RMS-MessageBodyType"];

		//		int length;
		//		if ( Int32.TryParse(headers["X-RMS-MessageBodyLength"], out length) )
		//			bodyInfo.Length = length;

		//		int fileSize;
		//		if ( Int32.TryParse(headers["X-RMS-MessageBodyFileSize"], out fileSize) )
		//			bodyInfo.FileSize = fileSize;

		//		return bodyInfo;
		//	}
		//	else
		//	{
		//		return null;
		//	}
		//}
		#endregion

	}
}
