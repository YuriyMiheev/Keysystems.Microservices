using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Xml.Serialization;

using Keysystems.RemoteMessaging.Lib.Collections;

namespace Keysystems.RemoteMessaging.DTO
{
	/// <summary>
	/// Информация о контенте сообщения.
	/// </summary>
	[Serializable]
	[XmlRoot(Namespace = XmlNamespaces.Rms, ElementName = "Content")]
	public class MessageContentInfo
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
		[XmlElement(ElementName = "Type", IsNullable = true)]
		public string Type
		{
			get { return type; }
			set { type = value; }
		}

		private int? length;
		/// <summary>
		/// {Get,Set} Размер содержимого.
		/// </summary>
		[XmlElement(ElementName = "Length", IsNullable = true)]
		public int? Length
		{
			get { return length; }
			set { length = value; }
		}

		private int? fileSize;
		/// <summary>
		/// {Get,Set} Размер файла.
		/// </summary>
		[XmlElement(ElementName = "FileSize", IsNullable = true)]
		public int? FileSize
		{
			get { return fileSize; }
			set { fileSize = value; }
		}

		private string comment;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		[XmlElement(ElementName = "Comment", IsNullable = true)]
		public string Comment
		{
			get { return comment; }
			set { comment = value; }
		}
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public NameValueCollection ToHeaders()
		{
			var headers = new NameValueCollection();
			headers.Add("X-RMS-MessageLINK", (this.MessageLINK == null ? null : this.MessageLINK.ToString()));
			headers.Add("X-RMS-MessageContentLINK", (this.LINK == null ? null : this.LINK.ToString()));
			headers.Add("X-RMS-MessageContentName", (this.Name == null ? null : HttpUtility.UrlPathEncode(this.Name)));
			headers.Add("X-RMS-MessageContentType", this.Type);
			headers.Add("X-RMS-MessageContentLength", (this.Length == null ? null : this.Length.ToString()));
			headers.Add("X-RMS-MessageContentFileSize", (this.FileSize == null ? null : this.FileSize.ToString()));
			headers.Add("X-RMS-MessageContentComment", (this.Comment == null ? null : HttpUtility.UrlPathEncode(this.Comment)));
			return headers;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="headers"></param>
		/// <returns></returns>
		public static MessageContentInfo Parse(NameValueCollection headers)
		{
			#region Validate parameters
			if ( headers == null )
				throw new ArgumentNullException("headers");
			#endregion

			if ( headers.HasKey("X-RMS-MessageLINK")
				|| headers.HasKey("X-RMS-MessageContentLINK")
				|| headers.HasKey("X-RMS-MessageContentName")
				|| headers.HasKey("X-RMS-MessageContentType")
				|| headers.HasKey("X-RMS-MessageContentLength")
				|| headers.HasKey("X-RMS-MessageContentFileSize")
				|| headers.HasKey("X-RMS-MessageContentComment") )
			{
				var contentInfo = new MessageContentInfo();

				int msgLink;
				if ( Int32.TryParse(headers["X-RMS-MessageLINK"], out msgLink) )
					contentInfo.MessageLINK = msgLink;

				int contentLink;
				if ( Int32.TryParse(headers["X-RMS-MessageContentLINK"], out contentLink) )
					contentInfo.LINK = contentLink;

				string name = headers["X-RMS-MessageContentName"];
				contentInfo.Name = (name == null ? null : HttpUtility.UrlDecode(name, Encoding.UTF8));
				contentInfo.Type = headers["X-RMS-MessageContentType"];

				int length;
				if ( Int32.TryParse(headers["X-RMS-MessageContentLength"], out length) )
					contentInfo.Length = length;

				int fileSize;
				if ( Int32.TryParse(headers["X-RMS-MessageContentFileSize"], out fileSize) )
					contentInfo.FileSize = fileSize;

				string comment = headers["X-RMS-MessageContentComment"];
				contentInfo.Comment = (comment == null ? null : HttpUtility.UrlDecode(comment, Encoding.UTF8));

				return contentInfo;
			}
			else
			{
				return null;
			}
		}
		#endregion

	}
}
