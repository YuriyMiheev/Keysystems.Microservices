using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Microservices.ChannelConnector
{
	/// <summary>
	/// Сообщение.
	/// </summary>
	[Serializable]
	//[XmlRoot(ElementName = "Message", Namespace = XmlNamespaces.Rms)]
	public class Message
	{

		#region Properties
		private int? link;
		/// <summary>
		/// {Get,Set} Внутренний ID.
		/// </summary>
		[XmlElement(ElementName = "LINK", IsNullable = true)]
		//[JsonProperty("LINK")]
		public int? LINK
		{
			get { return link; }
			set { link = value; }
		}

		private string channel;
		/// <summary>
		/// {Get} Ссылка на канал.
		/// </summary>
		[XmlElement(ElementName = "Channel", IsNullable = true)]
		//[JsonProperty("Channel")]
		public string Channel
		{
			get { return channel; }
			set { channel = value; }
		}

		//private string queue;
		///// <summary>
		///// {Get} Имя очереди.
		///// </summary>
		//[XmlElement(ElementName = "Queue", IsNullable = true)]
		//[JsonProperty("Queue")]
		//public string Queue
		//{
		//   get { return queue; }
		//   set { queue = value; }
		//}

		private string guid;
		/// <summary>
		/// {Get,Set} Глобальный ID.
		/// </summary>
		[XmlElement(ElementName = "GUID")]
		//[JsonProperty("GUID")]
		public string GUID
		{
			get { return guid; }
			set { guid = value; }
		}

		private string direction;
		/// <summary>
		/// {Get,Set} Направление передачи.
		/// </summary>
		[XmlElement(ElementName = "Direction")]
		//[JsonProperty("Direction")]
		public string Direction
		{
			get { return direction; }
			set { direction = value; }
		}

		private bool? async;
		/// <summary>
		/// {Get} Асинхронное сообщение.
		/// </summary>
		[XmlElement(ElementName = "Async", IsNullable = true)]
		//[JsonProperty("Async")]
		public bool? Async
		{
			get { return async; }
			set { async = value; }
		}

		private int? corrLink;
		/// <summary>
		/// {Get,Set} Ссылка на исходное сообщение.
		/// </summary>
		[XmlElement(ElementName = "CorrLINK", IsNullable = true)]
		//[JsonProperty("CorrLINK")]
		public int? CorrLINK
		{
			get { return corrLink; }
			set { corrLink = value; }
		}

		private string corrGuid;
		/// <summary>
		/// {Get,Set} Ссылка на исходное сообщение.
		/// </summary>
		[XmlElement(ElementName = "CorrGUID", IsNullable = true)]
		//[JsonProperty("CorrGUID")]
		public string CorrGUID
		{
			get { return corrGuid; }
			set { corrGuid = value; }
		}

		private string version;
		/// <summary>
		/// {Get,Set} Версия формата.
		/// </summary>
		[XmlElement(ElementName = "Version", IsNullable = true)]
		//[JsonProperty("Version")]
		public string Version
		{
			get { return version; }
			set { version = value; }
		}

		private string _class;
		/// <summary>
		/// {Get,Set} Класс.
		/// </summary>
		[XmlElement(ElementName = "Class", IsNullable = true)]
		//[JsonProperty("Class")]
		public string Class
		{
			get { return _class; }
			set { _class = value; }
		}

		private string type;
		/// <summary>
		/// {Get,Set} Тип.
		/// </summary>
		[XmlElement(ElementName = "Type", IsNullable = true)]
		//[JsonProperty("Type")]
		public string Type
		{
			get { return type; }
			set { type = value; }
		}

		private string name;
		/// <summary>
		/// {Get,Set} Имя.
		/// </summary>
		[XmlElement(ElementName = "Name", IsNullable = true)]
		//[JsonProperty("Name")]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string subject;
		/// <summary>
		/// {Get,Set} Тема.
		/// </summary>
		[XmlElement(ElementName = "Subject", IsNullable = true)]
		//[JsonProperty("Subject")]
		public string Subject
		{
			get { return subject; }
			set { subject = value; }
		}

		private DateTime? date;
		/// <summary>
		/// {Get,Set} Дата.
		/// </summary>
		[XmlElement(ElementName = "Date", IsNullable = true)]
		//[JsonProperty("Date")]
		public DateTime? Date
		{
			get { return date; }
			set { date = value; }
		}

		private DateTime? ttl;
		/// <summary>
		/// {Get,Set} Время жизни.
		/// </summary>
		[XmlElement(ElementName = "TTL", IsNullable = true)]
		//[JsonProperty("TTL")]
		public DateTime? TTL
		{
			get { return ttl; }
			set { ttl = value; }
		}

		private string from;
		/// <summary>
		/// {Get,Set} Отправитель.
		/// </summary>
		[XmlElement(ElementName = "From", IsNullable = true)]
		//[JsonProperty("From")]
		public string From
		{
			get { return from; }
			set { from = value; }
		}

		private string to;
		/// <summary>
		/// {Get,Set} Получатель(и).
		/// </summary>
		[XmlElement(ElementName = "To")]
		//[JsonProperty("To")]
		public string To
		{
			get { return to; }
			set { to = value; }
		}

		private string proxy;
		/// <summary>
		/// {Get,Set} Посредник(и).
		/// </summary>
		[XmlElement(ElementName = "Proxy", IsNullable = true)]
		//[JsonProperty("Proxy")]
		public string Proxy
		{
			get { return proxy; }
			set { proxy = value; }
		}

		private int? priority;
		/// <summary>
		/// {Get,Set} Приоритет.
		/// </summary>
		[XmlElement(ElementName = "Priority", IsNullable = true)]
		//[JsonProperty("Priority")]
		public int? Priority
		{
			get { return priority; }
			set { priority = value; }
		}

		private MessageBodyInfo body;
		/// <summary>
		/// {Get,Set} Тело сообщения.
		/// </summary>
		[XmlElement(ElementName = "Body", IsNullable = true)]
		//[JsonProperty("Body")]
		public MessageBodyInfo Body
		{
			get { return body; }
			set { body = value; }
		}

		private List<MessageProperty> properties;
		/// <summary>
		/// {Get,Set} Дополнительные свойства.
		/// </summary>
		[XmlElement(ElementName = "Property", IsNullable = true)]
		//[JsonProperty("Properties")]
		public List<MessageProperty> Properties
		{
			get { return properties = (properties ?? new List<MessageProperty>()); }
			set { properties = value; }
		}

		private List<MessageContentInfo> contents;
		/// <summary>
		/// {Get,Set} Содержимое.
		/// </summary>
		[XmlElement(ElementName = "Content", IsNullable = true)]
		//[JsonProperty("Contents")]
		public List<MessageContentInfo> Contents
		{
			get { return contents = (contents ?? new List<MessageContentInfo>()); }
			set { contents = value; }
		}

		//private List<MessagePost> posts;
		///// <summary>
		///// {Get,Set} Посылки.
		///// </summary>
		//[XmlElement(ElementName = "Post", IsNullable = true)]
		////[JsonProperty("Posts")]
		//public List<MessagePost> Posts
		//{
		//	get { return posts = (posts ?? new List<MessagePost>()); }
		//	set { posts = value; }
		//}

		private MessageStatus status;
		/// <summary>
		/// {Get,Set} Статус.
		/// </summary>
		[XmlElement(ElementName = "Status", IsNullable = true)]
		//[JsonProperty("Status")]
		public MessageStatus Status
		{
			get { return status = (status ?? new MessageStatus()); }
			set { status = value; }
		}
		#endregion

	}
}
