using System;
using System.Linq;
using System.Linq.Expressions;

using AutoMapper;
using AutoMapper.Configuration;

using Microservices;
using DAO = Microservices.Data.DAO;
//using DTO = Microservices.Channels.DTO;

namespace Microservices.Data
{
	/// <summary>
	/// 
	/// </summary>
	public static class ObjectConverter
	{
		private static IMapper mapper;

		/// <summary>
		/// Type initializer.
		/// </summary>
		static ObjectConverter()
		{
			var config = new MapperConfiguration(cfg =>
				{
					#region Message
					cfg.CreateMap<Message, Message>();
					#endregion

					#region MessageBody
					cfg.CreateMap<MessageBodyInfo, MessageBodyInfo>();
					cfg.CreateMap<MessageBody, MessageBody>();
					cfg.CreateMap<MessageBody, MessageBodyInfo>();
					cfg.CreateMap<MessageBodyInfo, MessageBody>();
					#endregion

					#region MessageProperty
					cfg.CreateMap<MessageProperty, MessageProperty>();
					#endregion

					#region MessageContent
					cfg.CreateMap<MessageContent, MessageContent>();
					cfg.CreateMap<MessageContentInfo, MessageContentInfo>();
					cfg.CreateMap<MessageContent, MessageContentInfo>();
					cfg.CreateMap<MessageContentInfo, MessageContent>();
					#endregion

					#region MessageStatus
					cfg.CreateMap<MessageStatus, MessageStatus>();
					#endregion
				});
			mapper = config.CreateMapper();
		}


		#region Message
		/// <summary>
		/// Создание копии объекта без LINK, CorrLINK, Body, Contents, Posts, Status и PrevStatus.
		/// </summary>
		/// <returns></returns>
		public static Message Copy(this Message src)
		{
			#region Validate parameters
			if ( src == null )
				throw new ArgumentNullException("src");
			#endregion

			Message dest = mapper.Map<Message, Message>(src);
			dest.LINK = 0;
			dest.CorrLINK = null;
			dest.Body = null;
			dest.Contents = null;
			//dest.Posts = null;
			dest.Status = null;
			dest.PrevStatus = null;
			dest.Properties = null;
			src.Properties.ToList().ForEach(prop => dest.AddNewProperty(prop.Copy()));

			return dest;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		/// <param name="dest"></param>
		public static void CloneTo(this Message src, Message dest)
		{
			#region Validate parameters
			if ( src == null )
				throw new ArgumentNullException("src");

			if ( dest == null )
				throw new ArgumentNullException("dest");
			#endregion

			mapper.Map<Message, Message>(src, dest);
		}

		#region DAO
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static DAO.Message ToDao(this Message obj)
		{
			if ( obj == null )
				return null;

			var dao = new DAO.Message();
			dao.Async = obj.Async ? obj.Async : new Nullable<bool>();
			dao.Body = obj.Body.ToDao();
			dao.Channel = (String.IsNullOrEmpty(obj.Channel) ? null : obj.Channel);
			dao.Class = (String.IsNullOrEmpty(obj.Class) ? null : obj.Class);
			dao.Contents = obj.Contents.Select(contentInfo => contentInfo.ToDao(dao)).ToList();
			dao.CorrLINK = obj.CorrLINK;
			dao.CorrGUID = (String.IsNullOrEmpty(obj.CorrGUID) ? null : obj.CorrGUID);
			dao.Date = obj.Date;
			dao.Direction = (String.IsNullOrEmpty(obj.Direction) ? null : obj.Direction);
			dao.From = (String.IsNullOrEmpty(obj.From) ? null : obj.From);
			dao.GUID = obj.GUID;
			dao.LINK = obj.LINK;
			dao.Name = (String.IsNullOrEmpty(obj.Name) ? null : obj.Name);
			//dao.Posts = obj.Posts.Select(post => post.ToDao(dao)).ToList();
			dao.Priority = (obj.Priority == 0 ? new Nullable<int>() : obj.Priority);
			dao.Properties = obj.Properties.Select(prop => prop.ToDao(dao)).ToList();
			dao.Proxy = (String.IsNullOrEmpty(obj.Proxy) ? null : obj.Proxy);
			//dao.Queue = obj.Queue;
			dao.Status = obj.Status.ToDao();
			dao.Subject = (String.IsNullOrEmpty(obj.Subject) ? null : obj.Subject);
			dao.To = (String.IsNullOrEmpty(obj.To) ? null : obj.To);
			dao.TTL = obj.TTL;
			dao.Type = (String.IsNullOrEmpty(obj.Type) ? null : obj.Type);
			dao.Version = (String.IsNullOrEmpty(obj.Version) ? null : obj.Version);

			return dao;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dao"></param>
		/// <returns></returns>
		public static Message ToObj(this DAO.Message dao)
		{
			if ( dao == null )
				return null;

			var obj = new Message();
			dao.CloneTo(obj);

			return obj;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dao"></param>
		/// <param name="obj"></param>
		public static void CloneTo(this DAO.Message dao, Message obj)
		{
			#region Validate parameters
			if ( dao == null )
				throw new ArgumentNullException("dao");

			if ( obj == null )
				throw new ArgumentNullException("obj");
			#endregion

			obj.LINK = dao.LINK;
			obj.Async = (dao.Async == null ? false : dao.Async.Value);
			obj.Body = dao.Body.ToObj();
			obj.Channel = dao.Channel;
			obj.Class = dao.Class;
			obj.Contents = dao.Contents.Select(contentInfo => contentInfo.ToObj()).ToArray();
			obj.CorrLINK = dao.CorrLINK;
			obj.CorrGUID = dao.CorrGUID;
			obj.Date = dao.Date;
			obj.Direction = dao.Direction;
			obj.From = dao.From;
			obj.GUID = dao.GUID;
			obj.Name = dao.Name;
			//obj.Posts = dao.Posts.Select(post => post.ToObj()).ToArray();
			obj.Priority = dao.Priority;
			obj.Properties = dao.Properties.Select(prop => prop.ToObj()).ToArray();
			obj.Proxy = dao.Proxy;
			//obj.Queue = dao.Queue;
			obj.Status = dao.Status.ToObj();
			obj.Subject = dao.Subject;
			obj.To = dao.To;
			obj.TTL = dao.TTL;
			obj.Type = dao.Type;
			obj.Version = dao.Version;
		}
		#endregion

		#endregion

		#region MessageBody
		/// <summary>
		/// Создание копии объекта без MessageLINK.
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static MessageBody Copy(this MessageBody src)
		{
			#region Validate parameters
			if ( src == null )
				throw new ArgumentNullException("src");
			#endregion

			//MessageBody dest = Mapper.Map<MessageBody, MessageBody>(src);
			//dest.MessageLINK = 0;
			//return dest;

			return new MessageBody()
			{
				Name = src.Name,
				Type = src.Type,
				Length = src.Length,
				FileSize = src.FileSize,
				Value = src.Value
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static MessageBodyInfo BodyInfo(this MessageBody body)
		{
			#region Validate parameters
			if ( body == null )
				throw new ArgumentNullException("body");
			#endregion

			return mapper.Map<MessageBody, MessageBodyInfo>(body);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="info"></param>
		public static void ApplyInfo(this MessageBody body, MessageBodyInfo info)
		{
			#region Validate parameters
			if ( body == null )
				throw new ArgumentNullException("body");

			if ( info == null )
				throw new ArgumentNullException("info");
			#endregion

			mapper.Map<MessageBodyInfo, MessageBody>(info, body);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		/// <param name="dest"></param>
		public static void CloneTo(this MessageBodyInfo src, MessageBodyInfo dest)
		{
			#region Validate parameters
			if ( src == null )
				throw new ArgumentNullException("src");

			if ( dest == null )
				throw new ArgumentNullException("dest");
			#endregion

			mapper.Map<MessageBodyInfo, MessageBodyInfo>(src, dest);
		}

		#region DAO
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static DAO.MessageBodyInfo ToDao(this MessageBodyInfo obj)
		{
			if ( obj == null )
				return null;

			var dao = new DAO.MessageBodyInfo();
			dao.FileSize = obj.FileSize;
			dao.Length = obj.Length;
			dao.MessageLINK = obj.MessageLINK;
			dao.Name = obj.Name;
			dao.Type = obj.Type;

			return dao;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dao"></param>
		/// <returns></returns>
		public static MessageBodyInfo ToObj(this DAO.MessageBodyInfo dao)
		{
			if ( dao == null )
				return null;

			var obj = new MessageBodyInfo();
			obj.FileSize = dao.FileSize;
			obj.Length = dao.Length;
			obj.MessageLINK = dao.MessageLINK;
			obj.Name = dao.Name;
			obj.Type = dao.Type;

			return obj;
		}
		#endregion

		#endregion

		#region MessageProperty
		/// <summary>
		/// Создание копии объекта без LINK и MessageLINK.
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static MessageProperty Copy(this MessageProperty src)
		{
			MessageProperty dest = mapper.Map<MessageProperty, MessageProperty>(src);
			dest.LINK = 0;
			dest.MessageLINK = null;

			return dest;
		}

		#region DAO
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public static DAO.MessageProperty ToDao(this MessageProperty obj, DAO.Message msg)
		{
			if ( obj == null )
				return null;

			#region Validate parameters
			if ( msg == null )
				throw new ArgumentNullException("msg");
			#endregion

			var dao = new DAO.MessageProperty();
			dao.Comment = obj.Comment;
			dao.Format = obj.Format;
			dao.LINK = obj.LINK;
			dao.Message = msg;
			dao.Name = obj.Name;
			dao.Type = obj.Type;
			dao.Value = obj.Value;

			return dao;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dao"></param>
		/// <returns></returns>
		public static MessageProperty ToObj(this DAO.MessageProperty dao)
		{
			if ( dao == null )
				return null;

			var obj = new MessageProperty();
			obj.Comment = dao.Comment;
			obj.Format = dao.Format;
			obj.LINK = dao.LINK;
			obj.MessageLINK = dao.Message.LINK;
			obj.Name = dao.Name;
			obj.Type = dao.Type;
			obj.Value = dao.Value;

			return obj;
		}
		#endregion

		#endregion

		#region MessageContent
		/// <summary>
		/// Создание копии без LINK и MessageLINK.
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static MessageContent Copy(this MessageContent src)
		{
			#region Validate parameters
			if ( src == null )
				throw new ArgumentNullException("src");
			#endregion

			//MessageContent dest = Mapper.Map<MessageContent, MessageContent>(src);
			//dest.LINK = 0;
			//dest.MessageLINK = null;
			//dest.Value = src.Value;
			//return dest;

			return new MessageContent()
			{
				Name = src.Name,
				Type = src.Type,
				Length = src.Length,
				FileSize = src.FileSize,
				Comment = src.Comment,
				Value = src.Value
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static MessageContentInfo ContentInfo(this MessageContent obj)
		{
			#region Validate parameters
			if ( obj == null )
				throw new ArgumentNullException("obj");
			#endregion

			return mapper.Map<MessageContent, MessageContentInfo>(obj);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="info"></param>
		public static void ApplyInfo(this MessageContent obj, MessageContentInfo info)
		{
			#region Validate parameters
			if ( obj == null )
				throw new ArgumentNullException("obj");

			if ( info == null )
				throw new ArgumentNullException("info");
			#endregion

			mapper.Map<MessageContentInfo, MessageContent>(info, obj);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		/// <param name="dest"></param>
		public static void CloneTo(this MessageContentInfo src, MessageContentInfo dest)
		{
			#region Validate parameters
			if ( src == null )
				throw new ArgumentNullException("src");

			if ( dest == null )
				throw new ArgumentNullException("dest");
			#endregion

			mapper.Map<MessageContentInfo, MessageContentInfo>(src, dest);
		}

		#region DAO
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public static DAO.MessageContentInfo ToDao(this MessageContentInfo obj, DAO.Message msg)
		{
			if ( obj == null )
				return null;

			#region Validate parameters
			if ( msg == null )
				throw new ArgumentNullException("msg");
			#endregion

			var dao = new DAO.MessageContentInfo();
			dao.Comment = obj.Comment;
			dao.FileSize = obj.FileSize;
			dao.Length = obj.Length;
			dao.LINK = obj.LINK;
			dao.Message = msg;
			dao.Name = obj.Name;
			dao.Type = obj.Type;

			return dao;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dao"></param>
		/// <returns></returns>
		public static MessageContentInfo ToObj(this DAO.MessageContentInfo dao)
		{
			if ( dao == null )
				return null;

			var obj = new MessageContentInfo();
			obj.Comment = dao.Comment;
			obj.FileSize = dao.FileSize;
			obj.Length = dao.Length;
			obj.LINK = dao.LINK;
			obj.MessageLINK = dao.Message.LINK;
			obj.Name = dao.Name;
			obj.Type = dao.Type;

			return obj;
		}
		#endregion

		#endregion

		#region MessageStatus
		/// <summary>
		/// Создание копии.
		/// </summary>
		/// <returns></returns>
		public static MessageStatus Clone(this MessageStatus src)
		{
			return mapper.Map<MessageStatus, MessageStatus>(src);
		}

		#region DAO
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static DAO.MessageStatus ToDao(this MessageStatus obj)
		{
			if ( obj == null )
				return null;

			var dao = new DAO.MessageStatus();
			dao.Code = obj.Code;
			dao.Date = obj.Date;
			dao.Info = obj.Info;
			dao.Value = (obj.Value == MessageStatus.DRAFT || String.IsNullOrWhiteSpace(obj.Value) ? null : obj.Value);

			return dao;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dao"></param>
		/// <returns></returns>
		public static MessageStatus ToObj(this DAO.MessageStatus dao)
		{
			if ( dao == null )
				return null;

			var obj = new MessageStatus();
			obj.Code = dao.Code;
			obj.Date = dao.Date;
			obj.Info = dao.Info;
			obj.Value = (dao.Value == MessageStatus.DRAFT || String.IsNullOrWhiteSpace(dao.Value) ? null : dao.Value);

			return obj;
		}
		#endregion

		#endregion

	}
}
