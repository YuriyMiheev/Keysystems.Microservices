using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.Data.DAO
{
	/// <summary>
	/// Сообщение.
	/// </summary>
	public class Message
	{
		/// <summary>
		/// 
		/// </summary>
		public Message()
		{
			this.Status = new MessageStatus();
		}


		#region Properties
		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual int LINK { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Channel { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string GUID { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Direction { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual bool? Async { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual int? CorrLINK { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string CorrGUID { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Version { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Class { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Type { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// {Get,Set} Тема.
		/// </summary>
		public virtual string Subject { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual DateTime? Date { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual DateTime? TTL { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string From { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string To { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Proxy { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual int? Priority { get; set; }

		///// <summary>
		///// {Get,Set} 
		///// </summary>
		//public virtual string Queue { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual MessageBodyInfo Body { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual IList<MessageProperty> Properties { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual IList<MessageContentInfo> Contents { get; set; }

		///// <summary>
		///// {Get,Set} 
		///// </summary>
		//public virtual IList<MessagePost> Posts { get; set; }

		private MessageStatus status;
		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual MessageStatus Status
		{
			get { return status; }
			set { status = (value ?? new MessageStatus()); }
		}
		#endregion

	}
}
