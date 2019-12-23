using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Microservices
{
	/// <summary>
	/// Сообщение (заголовок сообщения).
	/// </summary>
	public class Message
	{

		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public Message()
		{
			this.LINK = 0;
			this.Channel = "";
			this.GUID = Guid.NewGuid().ToString();
			this.Direction = "";
			this.Version = MessageVersion.Current;
			this.Class = "";
			this.Type = MessageType.DOCUMENT;
			this.Name = "";
			this.Subject = "";
			this.Date = DateTime.Now;
			this.From = "";
			this.To = "";
			this.Proxy = "";
			this.Properties = new MessageProperty[0];
			this.Contents = new MessageContentInfo[0];
			//this.Posts = new MessagePost[0];
			this.Status = new MessageStatus();
			this.PrevStatus = new MessageStatus();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="direction"></param>
		/// <param name="msgClass"></param>
		/// <param name="msgType"></param>
		public Message(string direction, string msgClass, string msgType)
			: this()
		{
			#region Validate parameters
			if ( String.IsNullOrEmpty(direction) )
				throw new ArgumentException("direction");

			if ( String.IsNullOrEmpty(msgClass) )
				throw new ArgumentException("msgClass");

			if ( String.IsNullOrEmpty(msgType) )
				throw new ArgumentException("msgType");
			#endregion

			this.Direction = direction;
			this.Class = msgClass;
			this.Type = msgType;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public int LINK { get; set; }

		private string channel;
		/// <summary>
		/// {Get,Set} Виртуальный адрес канала, которому принадлежит сообщение.
		/// </summary>
		public string Channel
		{
			get { return channel; }
			set { channel = (value ?? "").ToLower(); }
		}

		/// <summary>
		/// {Get,Set} Глобальный ID.
		/// </summary>
		public string GUID { get; set; }

		private string direction;
		/// <summary>
		/// {Get,Set} Направление передачи.
		/// </summary>
		public string Direction
		{
			get { return direction; }
			set { direction = (value ?? ""); }
		}

		/// <summary>
		/// {Get,Set} Асинхронное сообщение.
		/// </summary>
		public bool Async { get; set; }

		/// <summary>
		/// {Get,Set} Ссылка на исходное сообщение.
		/// </summary>
		public int? CorrLINK { get; set; }

		/// <summary>
		/// {Get,Set} Ссылка на исходное сообщение.
		/// </summary>
		public string CorrGUID { get; set; }

		private string version;
		/// <summary>
		/// {Get,Set} Версия формата сообщения.
		/// </summary>
		public string Version
		{
			get { return version; }
			set { version = (value ?? ""); }
		}

		private string _class;
		/// <summary>
		/// {Get,Set} Класс.
		/// </summary>
		public string Class
		{
			get { return _class; }
			set { _class = (value ?? ""); }
		}

		private string type;
		/// <summary>
		/// {Get,Set} Тип.
		/// </summary>
		public string Type
		{
			get { return type; }
			set { type = (value ?? ""); }
		}

		private string name;
		/// <summary>
		/// {Get,Set} Имя.
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = (value ?? ""); }
		}

		private string subject;
		/// <summary>
		/// {Get,Set} Тема.
		/// </summary>
		public string Subject
		{
			get { return subject; }
			set { subject = (value ?? ""); }
		}

		/// <summary>
		/// {Get,Set} Дата.
		/// </summary>
		public DateTime? Date { get; set; }

		/// <summary>
		/// {Get,Set} Время жизни.
		/// </summary>
		public DateTime? TTL { get; set; }

		private string from;
		/// <summary>
		/// {Get,Set} Отправитель.
		/// </summary>
		public string From
		{
			get { return from; }
			set { from = (value ?? ""); }
		}

		private string to;
		/// <summary>
		/// {Get,Set} Получатель(и).
		/// </summary>
		public string To
		{
			get { return to; }
			set { to = (value ?? ""); }
		}

		private string proxy;
		/// <summary>
		/// {Get,Set} Посредник(и).
		/// </summary>
		public string Proxy
		{
			get { return proxy; }
			set { proxy = (value ?? ""); }
		}

		/// <summary>
		/// {Get,Set} Приоритет.
		/// </summary>
		public int? Priority { get; set; }

		private MessageBodyInfo body;
		/// <summary>
		/// {Get,Set} Тело.
		/// </summary>
		public MessageBodyInfo Body
		{
			get { return body; }
			set
			{
				body = value;
				if ( body != null )
					body.MessageLINK = this.LINK;
			}
		}

		private List<MessageProperty> properties;
		/// <summary>
		/// {Get} Дополнительные св-ва.
		/// </summary>
		public MessageProperty[] Properties
		{
			get { return properties.ToArray(); }
			set { properties = (value ?? new MessageProperty[0]).ToList(); }
		}

		private List<MessageContentInfo> contents;
		/// <summary>
		/// {Get} Содержимое.
		/// </summary>
		public MessageContentInfo[] Contents
		{
			get { return contents.ToArray(); }
			set { contents = (value ?? new MessageContentInfo[0]).ToList(); }
		}

		//private List<MessagePost> posts;
		///// <summary>
		///// {Get} Посылки.
		///// </summary>
		//public MessagePost[] Posts
		//{
		//	get { return posts.ToArray(); }
		//	internal set { posts = (value ?? new MessagePost[0]).ToList(); }
		//}

		private MessageStatus status;
		/// <summary>
		/// {Get} Статус.
		/// </summary>
		public MessageStatus Status
		{
			get { return status; }
			set { status = (value ?? new MessageStatus()); }
		}

		private MessageStatus prevStatus;
		/// <summary>
		/// {Get} Предыдущий статус.
		/// </summary>
		public MessageStatus PrevStatus
		{
			get { return prevStatus; }
			set { prevStatus = (value ?? new MessageStatus()); }
		}
		#endregion


		#region Methods
		/// <summary>
		/// Добавление нового св-ва.
		/// </summary>
		/// <param name="prop"></param>
		public void AddNewProperty(MessageProperty prop)
		{
			#region Validate parameters
			if ( prop == null )
				throw new ArgumentNullException("prop");

			if ( String.IsNullOrEmpty(prop.Name) )
				throw new ArgumentException("Отсутствует имя свойства.", "prop");

			if ( prop.LINK != 0 )
				throw new ArgumentException("Свойство не должно иметь LINK.", "prop");
			#endregion

			lock ( this.properties )
			{
				if ( this.properties.Any(p => p.Name == prop.Name) )
					throw new InvalidOperationException(String.Format("Сообщение уже содержит свойство {0}.", prop.Name));

				prop.MessageLINK = this.LINK;
				this.properties.Add(prop);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propLink"></param>
		/// <returns></returns>
		public MessageProperty FindProperty(int propLink)
		{
			lock ( this.properties )
			{
				return this.properties.SingleOrDefault(prop => prop.LINK == propLink);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propLink"></param>
		/// <exception cref="MessagePropertyNotFoundException"></exception>
		/// <returns></returns>
		public MessageProperty GetProperty(int propLink)
		{
			MessageProperty prop = FindProperty(propLink);
			if ( prop == null )
				throw new MessagePropertyNotFoundException(propLink, this.LINK);

			return prop;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propName"></param>
		/// <returns></returns>
		public MessageProperty FindProperty(string propName)
		{
			#region Validate parameters
			if ( String.IsNullOrEmpty(propName) )
				throw new ArgumentException("propName");
			#endregion

			lock ( this.properties )
			{
				return this.properties.SingleOrDefault(prop => prop.Name == propName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propName"></param>
		/// <exception cref="MessagePropertyNotFoundException"></exception>
		/// <returns></returns>
		public MessageProperty GetProperty(string propName)
		{
			MessageProperty prop = FindProperty(propName);
			if ( prop == null )
				throw new MessagePropertyNotFoundException(propName, this.LINK);

			return prop;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propName"></param>
		public void RemoveProperty(string propName)
		{
			#region Validate parameters
			if ( String.IsNullOrEmpty(propName) )
				throw new ArgumentException("propName");
			#endregion

			MessageProperty prop = FindProperty(propName);
			if ( prop != null )
			{
				lock ( this.properties )
				{
					this.properties.Remove(prop);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentName"></param>
		/// <returns></returns>
		public MessageContentInfo FindContent(string contentName)
		{
			#region Validate parameters
			if ( String.IsNullOrEmpty(contentName) )
				throw new ArgumentException("contentName");
			#endregion

			lock ( this.contents )
			{
				return this.contents.SingleOrDefault(content => content.Name == contentName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentName"></param>
		/// <exception cref="MessageContentNotFoundException"></exception>
		/// <returns></returns>
		public MessageContentInfo GetContent(string contentName)
		{
			MessageContentInfo content = FindContent(contentName);
			if ( content == null )
				throw new MessageContentNotFoundException(this.LINK, contentName);

			return content;
		}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="post"></param>
		//public void AddNewPost(MessagePost post)
		//{
		//	#region Validate parameters
		//	if ( post == null )
		//		throw new ArgumentNullException("post");
		//	#endregion

		//	if ( post.LINK != 0 )
		//		throw new ArgumentException("Посылка должна иметь LINK равный 0.");

		//	post.MessageLINK = this.LINK;

		//	lock ( this.posts )
		//	{
		//		this.posts.Add(post);
		//	}
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="virtAddress"></param>
		///// <returns></returns>
		//public MessagePost FindPost(string virtAddress)
		//{
		//	lock ( this.posts )
		//	{
		//		return this.posts.SingleOrDefault(p => p.Address.Equals(virtAddress, StringComparison.InvariantCultureIgnoreCase));
		//	}
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="virtAddress"></param>
		///// <exception cref="MessagePostNotFoundException"></exception>
		///// <returns></returns>
		//public MessagePost GetPost(string virtAddress)
		//{
		//	lock ( this.posts )
		//	{
		//		MessagePost post = this.posts.SingleOrDefault(p => p.Address.Equals(virtAddress, StringComparison.InvariantCultureIgnoreCase));
		//		if ( post == null )
		//			throw new MessagePostNotFoundException(String.Format("Посылка сообщения с адресом \"{0}\" не найдена.", virtAddress));

		//		return post;
		//	}
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="postLink"></param>
		///// <returns></returns>
		//public MessagePost FindPost(int postLink)
		//{
		//	lock ( this.posts )
		//	{
		//		return this.posts.SingleOrDefault(p => p.LINK == postLink);
		//	}
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="postLink"></param>
		///// <exception cref="MessagePostNotFoundException"></exception>
		///// <returns></returns>
		//public MessagePost GetPost(int postLink)
		//{
		//	lock ( this.posts )
		//	{
		//		MessagePost post = this.posts.SingleOrDefault(p => p.LINK == postLink);
		//		if ( post == null )
		//			throw new MessagePostNotFoundException(String.Format("Посылка сообщения #{0} не найдена.", postLink));

		//		return post;
		//	}
		//}

		///// <summary>
		///// 
		///// </summary>
		//public void ClearPosts()
		//{
		//	lock ( this.posts )
		//	{
		//		this.posts.Clear();
		//	}
		//}

		/// <summary>
		/// Установить статус.
		/// </summary>
		/// <param name="status"></param>
		public void SetStatus(MessageStatus status)
		{
			#region Validate parameters
			if ( status == null )
				throw new ArgumentNullException("status");
			#endregion

			this.PrevStatus = this.Status;
			this.Status = status;
		}

		/// <summary>
		/// Установить статус.
		/// </summary>
		/// <param name="status"></param>
		public void SetStatus(string status)
		{
			var newStatus = new MessageStatus()
				{
					Value = status,
					Date = DateTime.Now,
					Info = null,
					Code = null
				};
			SetStatus(newStatus);
		}

		/// <summary>
		/// Установить статус.
		/// </summary>
		/// <param name="status"></param>
		/// <param name="code"></param>
		public void SetStatus(string status, int? code)
		{
			SetStatus(status);
			this.Status.Code = code;
		}

		/// <summary>
		/// Установить статус.
		/// </summary>
		/// <param name="status"></param>
		/// <param name="info"></param>
		public void SetStatus(string status, string info)
		{
			SetStatus(status);
			this.Status.Info = info;
			this.Status.Code = null;
		}

		/// <summary>
		/// Установить статус.
		/// </summary>
		/// <param name="status"></param>
		/// <param name="code"></param>
		/// <param name="info"></param>
		public void SetStatus(string status, int? code, string info)
		{
			SetStatus(status);
			this.Status.Info = info;
			this.Status.Code = code;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<string> GetRecipients()
		{
			return this.To.Split(new char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<string> GetProxies()
		{
			return this.Proxy.Split(new char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("#{0}", this.LINK);
		}
		#endregion

	}
}
