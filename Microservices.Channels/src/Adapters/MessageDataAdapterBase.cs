using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mime;
using System.Text;

using NHibernate;
using NHibernate.Criterion;

using Microservices.Channels;
using Microservices.Channels.Data;

namespace Microservices.Channels.Adapters
{
	/// <summary>
	/// Базовый класс для доступа к БД.
	/// </summary>
	public abstract class MessageDataAdapterBase
	{
		private IDatabase _database;
		private DbContext _dbContext;
		private int bufferSize = 4096 * 1024; //ServiceEnvironment.MessageBufferSize;


		#region Ctor
		/// <summary>
		/// Создание экземпляра.
		/// </summary>
		/// <param name="database"></param>
		protected MessageDataAdapterBase(IDatabase database)
		{
			_database = database ?? throw new ArgumentNullException(nameof(database));
			this.ExecuteTimeout = 30;
		}

		/// <summary>
		/// Создание экземпляра.
		/// </summary>
		/// <param name="dbContext"></param>
		protected MessageDataAdapterBase(DbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			this.ExecuteTimeout = 30;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Контекст подключения к БД.
		/// </summary>
		public DbContext DbContext
		{
			get
			{
				if (_dbContext == null)
					_dbContext = _database.Open();

				return _dbContext;
			}
		}

		/// <summary>
		/// {Get,Set} Таймаут выполнения (сек).
		/// </summary>
		public int ExecuteTimeout { get; set; }
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="error"></param>
		/// <returns></returns>
		public virtual bool CheckConnection(out ConnectionException error)
		{
			return this.DbContext.Database.TryConnect(out error);
		}

		/// <summary>
		/// Открыть запрос.
		/// </summary>
		/// <returns></returns>
		public virtual IDataQuery OpenQuery()
		{
			return this.DbContext.OpenQuery();
		}

		/// <summary>
		/// Открыть запрос.
		/// </summary>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public virtual IDataQuery OpenQuery(IsolationLevel transaction)
		{
			return this.DbContext.OpenQuery(transaction);
		}

		/// <summary>
		/// Начать работу.
		/// </summary>
		/// <returns></returns>
		public virtual UnitOfWork BeginWork()
		{
			return this.DbContext.BeginWork();
		}

		/// <summary>
		/// Начать работу.
		/// </summary>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public virtual UnitOfWork BeginWork(IsolationLevel transaction)
		{
			return this.DbContext.BeginWork(transaction);
		}


		#region Messages
		/// <summary>
		/// Выборка сообщений.
		/// </summary>
		/// <param name="dataQuery"></param>
		/// <returns></returns>
		public virtual IQueryOver<DAO.Message, DAO.Message> QueryMessages(IDataQuery dataQuery)
		{
			#region Validate parameters
			if ( dataQuery == null )
				throw new ArgumentNullException("dataQuery");
			#endregion

			return dataQuery.Open<DAO.Message>();
		}

		public virtual int ExecuteUpdate(string sql)
		{
			using (IDataQuery dataQuery = OpenQuery())
			{
				ISQLQuery sqlQuery = dataQuery.CreateSQLQuery(sql);
				return sqlQuery.ExecuteUpdate();
			}
		}

		/// <summary>
		/// Выборка сообщений.
		/// </summary>
		/// <param name="queryParams">Параметры.</param>
		/// <returns></returns>
		public virtual List<Message> SelectMessages(QueryParams queryParams)
		{
			#region Validate parameters
			if ( queryParams == null )
				throw new ArgumentNullException("queryParams");

			if ( String.IsNullOrEmpty(queryParams.Query) )
				throw new ArgumentException("Отсутствует текст запроса.", "queryParams");
			#endregion

			using ( IDataQuery dataQuery = OpenQuery() )
			{
				IQuery query;
				if ( queryParams.Params is IDictionary<string, object> )
				{
					query = dataQuery.Open(queryParams.Query);
					foreach ( KeyValuePair<string, object> pair in (IDictionary<string, object>)queryParams.Params )
					{
						query.SetParameter(pair.Key, pair.Value);
					}
				}
				else if ( queryParams.Params is IEnumerable<object> )
				{
					query = dataQuery.Open(queryParams.Query);
					for ( int i = 0; i < ((IEnumerable<object>)queryParams.Params).Count(); i++ )
					{
						object value = ((IEnumerable<object>)queryParams.Params).ToArray()[i];
						query.SetParameter(i, value);
					}
				}
				else if ( queryParams.Params == null )
				{
					query = dataQuery.Open(queryParams.Query);
				}
				else
				{
					query = dataQuery.Open(queryParams.Query);
					query.SetProperties(queryParams.Params);
				}

				if ( queryParams.Skip != null && queryParams.Skip > 0 )
					query.SetFirstResult(queryParams.Skip.Value);

				if ( queryParams.Take != null )
					query.SetMaxResults(queryParams.Take.Value);

				IList list = query.List();
				if ( list.Count == 0 )
				{
					return new List<Message>();
				}
				else
				{
					if ( list[0] is DAO.Message )
						return list.OfType<DAO.Message>().Select(msg => msg.ToObj()).ToList();
					else if ( list[0] is Array )
						return list.OfType<object[]>().Select(arr => ((DAO.Message)arr[0]).ToObj()).ToList();
					else
						return new List<Message>();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="status"></param>
		/// <param name="skip"></param>
		/// <param name="take"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		public virtual List<Message> GetMessages(string status, int? skip, int? take, out int totalCount)
		{
			using ( IDataQuery dataQuery = OpenQuery() )
			{
				var query = dataQuery.Open<DAO.Message>();
				//if (String.IsNullOrEmpty(channel))
				//	query.Where(msg => msg.Channel == null || msg.Channel == "");
				//else if (channel != "*")
				//	query.Where(msg => msg.Channel == channel);

				if ( status == MessageStatus.DRAFT )
				{
					query.AndRestrictionOn(msg => msg.Status.Value).IsNull();
				}
				else if ( !String.IsNullOrWhiteSpace(status) )
				{
					string[] statuses = status.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
					query.AndRestrictionOn(msg => msg.Status.Value).IsIn(statuses);
				}
				totalCount = query.RowCount();


				//query = dataQuery.Open<DAO.Message>();
				//if ( status == MessageStatus.DRAFT )
				//{
				//	query.AndRestrictionOn(msg => msg.Status.Value).IsNull();
				//}
				//else if ( !String.IsNullOrWhiteSpace(status) )
				//{
				//	string[] statuses = status.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
				//	query.AndRestrictionOn(msg => msg.Status.Value).IsIn(statuses);
				//}

				if ( skip != null && skip > 0 )
					query.Skip(skip.Value);

				if ( take != null )
					query.Take(take.Value);

				//if ( String.IsNullOrEmpty(channel) )
				//	query.Where(msg => msg.Channel == null || msg.Channel == "");
				//else if ( channel != "*" )
				//	query.Where(msg => msg.Channel == channel);

				query.OrderBy(msg => msg.LINK);
				return query.List().Select(msg => msg.ToObj()).ToList();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="status"></param>
		/// <param name="skip"></param>
		/// <param name="take"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		public virtual List<Message> GetLastMessages(string status, int? skip, int? take, out int totalCount)
		{
			using ( IDataQuery dataQuery = OpenQuery() )
			{
				var query = dataQuery.Open<DAO.Message>();
				//if (String.IsNullOrEmpty(channel))
				//	query.Where(msg => msg.Channel == null || msg.Channel == "");
				//else if (channel != "*")
				//	query.Where(msg => msg.Channel == channel);

				if ( status == MessageStatus.DRAFT )
				{
					query.AndRestrictionOn(msg => msg.Status.Value).IsNull();
				}
				else if ( !String.IsNullOrWhiteSpace(status) )
				{
					string[] statuses = status.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
					query.AndRestrictionOn(msg => msg.Status.Value).IsIn(statuses);
				}
				totalCount = query.RowCount();


				//query = dataQuery.Open<DAO.Message>();
				//if ( status == MessageStatus.DRAFT )
				//{
				//	query.AndRestrictionOn(msg => msg.Status.Value).IsNull();
				//}
				//else if ( !String.IsNullOrWhiteSpace(status) )
				//{
				//	string[] statuses = status.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
				//	query.AndRestrictionOn(msg => msg.Status.Value).IsIn(statuses);
				//}

				if ( skip != null && skip > 0 )
					query.Skip(skip.Value);

				if ( take != null )
					query.Take(take.Value);

				//if ( String.IsNullOrEmpty(channel) )
				//	query.Where(msg => msg.Channel == null || msg.Channel == "");
				//else if ( channel != "*" )
				//	query.Where(msg => msg.Channel == channel);

				query.OrderBy(msg => msg.LINK).Desc();
				return query.List().Select(msg => msg.ToObj()).ToList();
			}
		}

		/// <summary>
		/// Получить сообщение.
		/// </summary>
		/// <param name="msgLink"></param>
		/// <returns></returns>
		public virtual Message GetMessage(int msgLink)
		{
			using ( IDataQuery dataQuery = OpenQuery() )
			{
				return dataQuery.Get<DAO.Message>(msgLink).ToObj();
			}
		}

		/// <summary>
		/// Сообщение по GUID и направлению.
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="msgGuid"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public virtual Message FindMessage(string msgGuid, string direction)
		{
			using ( IDataQuery dataQuery = OpenQuery() )
			{
				var query = dataQuery.Open<DAO.Message>();
				//if ( String.IsNullOrEmpty(channel) )
				//	query.Where(msg => msg.Channel == null || msg.Channel == "");
				//else if ( channel != "*" )
				//	query.Where(msg => (msg.Channel == channel));

				query.Where(msg => (msg.GUID == msgGuid && msg.Direction == direction));
				return query.SingleOrDefault().ToObj();
			}
		}

		/// <summary>
		/// Сохранить сообщение.
		/// </summary>
		/// <param name="msg"></param>
		public virtual void SaveMessage(Message msg)
		{
			#region Validate parameters
			if ( msg == null )
				throw new ArgumentNullException("msg");
			#endregion

			DAO.Message dao = msg.ToDao();

			using ( UnitOfWork work = BeginWork() )
			{
				if ( dao.LINK == 0 )
					work.Save(dao);
				else
					work.Update<DAO.Message>(ref dao);

				work.End();
			}

			dao.CloneTo(msg);
		}

		/// <summary>
		/// Удалить сообщение.
		/// </summary>
		/// <param name="msgLink"></param>
		public virtual void DeleteMessage(int msgLink)
		{
			using ( UnitOfWork work = BeginWork() )
			{
				var dao = work.Get<DAO.Message>(msgLink);
				if ( dao == null )
					return;

				work.Delete(dao);
				work.End();
			}
		}

		///// <summary>
		///// Удалить удаленные сообщения.
		///// </summary>
		///// <param name="channel">* - все.</param>
		//public virtual void DeleteDeletedMessages(string channel)
		//{
		//	var msgLinks = new List<int>();
		//	using ( IDataQuery dataQuery = OpenQuery() )
		//	{
		//		var query = QueryMessages(dataQuery)
		//			.Where(msg => msg.Status.Value == MessageStatus.DELETED);

		//		if ( String.IsNullOrEmpty(channel) )
		//			query.Where(msg => msg.Channel == null || msg.Channel == "");
		//		else if ( channel != "*" )
		//			query.Where(msg => msg.Channel == channel);

		//		msgLinks = query.List().Select(msg => msg.LINK).ToList();
		//	}

		//	if ( msgLinks.Count > 0 )
		//	{
		//		msgLinks.Sort();
		//		DeleteMessages(msgLinks);
		//	}
		//}

		///// <summary>
		///// Удалить устаревшие сообщения.
		///// System.InvalidOperationException: Ошибка удаления устаревших сообщений. ---> System.Exception: Cannot supply null value to operator LessThanOrEqual
		///// </summary>
		///// <param name="channel">* - все.</param>
		///// <param name="expiredDate"></param>
		///// <param name="statuses"></param>
		//public virtual void DeleteExpiredMessages(string channel, DateTime expiredDate, List<string> statuses)
		//{
		//	var msgLinks = new List<int>();
		//	using ( IDataQuery dataQuery = OpenQuery() )
		//	{
		//		var query = QueryMessages(dataQuery)
		//			.Where(msg => (msg.TTL == null && msg.Date <= expiredDate) || (msg.TTL <= DateTime.Now))
		//			.AndRestrictionOn(msg => msg.Status.Value).IsIn(statuses);

		//		if ( String.IsNullOrEmpty(channel) )
		//			query.Where(msg => msg.Channel == null || msg.Channel == "");
		//		else if ( channel != "*" )
		//			query.Where(msg => msg.Channel == channel);

		//		msgLinks = query.List().Select(msg => msg.LINK).ToList();
		//	}

		//	if ( msgLinks.Count > 0 )
		//	{
		//		msgLinks.Sort();
		//		DeleteMessages(msgLinks);
		//	}
		//}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLinks"></param>
		public virtual void DeleteMessages(IEnumerable<int> msgLinks)
		{
			#region Validate parameters
			if ( msgLinks == null )
				throw new ArgumentNullException("msgLinks");
			#endregion

			int count = msgLinks.Count();
			if ( count > 0 )
			{
				int skip = 0;
				while ( skip < count )
				{
					List<int> links = msgLinks.Skip(skip).Take(1000).ToList();
					skip += links.Count;

					using ( UnitOfWork work = BeginWork() )
					{
						ISQLQuery query = work.CreateSQLQuery(String.Format("DELETE FROM {0} WHERE MESSAGE_LINK IN (:messages)", Database.Tables.MESSAGE_PROPERTIES));
						query.SetParameterList("messages", links);
						query.ExecuteUpdate();

						query = work.CreateSQLQuery(String.Format("DELETE FROM {0} WHERE MESSAGE_LINK IN (:messages)", Database.Tables.MESSAGE_CONTENTS));
						query.SetParameterList("messages", links);
						query.ExecuteUpdate();

						//query = work.CreateSQLQuery(String.Format("DELETE FROM {0} WHERE MESSAGE_LINK IN (:messages)", Database.Tables.MESSAGE_POSTS));
						//query.SetParameterList("messages", links);
						//query.ExecuteUpdate();

						query = work.CreateSQLQuery(String.Format("DELETE FROM {0} WHERE LINK IN (:messages)", Database.Tables.MESSAGES));
						query.SetParameterList("messages", links);
						query.ExecuteUpdate();

						work.End();
					}
				}
			}
		}


		#region Body
		/// <summary>
		/// Получить тело сообщения.
		/// </summary>
		/// <param name="msgLink"></param>
		/// <returns></returns>
		public virtual MessageBody GetMessageBody(int msgLink)
		{
			UnitOfWork work = BeginWork();

			MessageBodyInfo bodyInfo = work.Get<DAO.MessageBodyInfo>(msgLink).ToObj();
			if ( bodyInfo == null )
				return null;

			MessageBodyStreamBase stream = ConstructMessageBodyStream(msgLink, work, DataStreamMode.READ, bodyInfo.ContentType().Encoding());
			stream.ReadTimeout = this.ExecuteTimeout;

			if ( bodyInfo.Length == null )
				bodyInfo.Length = (int)stream.Length;

			var body = new MessageBody();
			body.ApplyInfo(bodyInfo);
			body.Value = new MessageBodyReader(stream, this.bufferSize);

			return body;
		}

		protected abstract MessageBodyStreamBase ConstructMessageBodyStream(int msgLink, UnitOfWork work, DataStreamMode mode, Encoding encoding);

		/// <summary>
		/// Сохранить тело сообщения.
		/// </summary>
		/// <param name="body"></param>
		public virtual void SaveMessageBody(MessageBody body)
		{
			#region Validate parameters
			if ( body == null )
				throw new ArgumentNullException("body");
			#endregion

			MessageBodyInfo bodyInfo = body.BodyInfo();
			ContentType contentType = bodyInfo.ContentType();
			Encoding encoding = contentType.Encoding();

			DAO.MessageBodyInfo dao = bodyInfo.ToDao();

			using ( UnitOfWork work = BeginWork() )
			{
				work.Update<DAO.MessageBodyInfo>(ref dao);

				using (MessageBodyStreamBase stream = ConstructMessageBodyStream(dao.MessageLINK, work, DataStreamMode.WRITE, encoding))
				{
					stream.WriteTimeout = this.ExecuteTimeout;

					var buffer = new char[this.bufferSize];
					int charsReaded;
					do
					{
						charsReaded = body.Value.Read(buffer, 0, buffer.Length);
						if ( charsReaded > 0 )
							stream.Write(buffer, 0, charsReaded);
					} while ( charsReaded > 0 );

					if ( dao.Length == null )
					{
						dao.Length = (int)stream.Length;
						work.Update<DAO.MessageBodyInfo>(ref dao);
					}

					work.End();
				}
			}

			bodyInfo = dao.ToObj();
			body.ApplyInfo(bodyInfo);
		}

		/// <summary>
		/// Удалить тело сообщения.
		/// </summary>
		/// <param name="msgLink"></param>
		public virtual void DeleteMessageBody(int msgLink)
		{
			using ( UnitOfWork work = BeginWork() )
			{
				var dao = work.Get<DAO.MessageBody>(msgLink);
				if ( dao == null )
					throw new MessageNotFoundException(msgLink);

				dao.FileSize = null;
				dao.Length = null;
				dao.Name = null;
				dao.Type = null;
				dao.Value = null;

				work.Update<DAO.MessageBody>(ref dao);
				work.End();
			}
		}
		#endregion


		#region Content
		/// <summary>
		/// Получить контент сообщения.
		/// </summary>
		/// <param name="contentLink"></param>
		/// <returns></returns>
		public virtual MessageContent GetMessageContent(int contentLink)
		{
			UnitOfWork work = BeginWork();

			MessageContentInfo contentInfo = work.Get<DAO.MessageContentInfo>(contentLink).ToObj();
			if ( contentInfo == null )
				return null;

			MessageContentStreamBase stream = ConstructMessageContentStream(contentInfo.LINK, work, DataStreamMode.READ, contentInfo.ContentType().Encoding());
			stream.ReadTimeout = this.ExecuteTimeout;

			if ( contentInfo.Length == null )
				contentInfo.Length = (int)stream.Length;

			var content = new MessageContent();
			content.ApplyInfo(contentInfo);
			content.Value = new MessageContentReader(stream, this.bufferSize);

			return content;
		}

		protected abstract MessageContentStreamBase ConstructMessageContentStream(int contentLink, UnitOfWork work, DataStreamMode mode, Encoding encoding);

		/// <summary>
		/// Сохранить контент сообщения.
		/// </summary>
		/// <param name="content"></param>
		public virtual void SaveMessageContent(MessageContent content)
		{
			#region Validate parameters
			if ( content == null )
				throw new ArgumentNullException("content");
			#endregion

			MessageContentInfo contentInfo = content.ContentInfo();
			ContentType contentType = contentInfo.ContentType();
			Encoding encoding = contentType.Encoding();

			using ( UnitOfWork work = BeginWork() )
			{
				int msgLink = content.MessageLINK.Value;
				var msg = work.Get<DAO.Message>(msgLink);
				if ( msg == null )
					throw new MessageNotFoundException(msgLink);

				DAO.MessageContentInfo dao = contentInfo.ToDao(msg);

				if ( dao.LINK == 0 )
					work.Save(dao);
				else
					work.Update<DAO.MessageContentInfo>(ref dao);

				if (content.Value != null)
				{
					using (MessageContentStreamBase stream = ConstructMessageContentStream(dao.LINK, work, DataStreamMode.WRITE, encoding))
					{
						stream.WriteTimeout = this.ExecuteTimeout;

						var buffer = new char[this.bufferSize];
						int charsReaded;
						do
						{
							charsReaded = content.Value.Read(buffer, 0, buffer.Length);
							if (charsReaded > 0)
								stream.Write(buffer, 0, charsReaded);
						} while (charsReaded > 0);

						if (dao.Length == null)
						{
							dao.Length = (int)stream.Length;
							work.Update<DAO.MessageContentInfo>(ref dao);
						}

						work.End();
					}
				}
				else
				{
					work.End();
				}

				contentInfo = dao.ToObj();
				content.ApplyInfo(contentInfo);
			}
		}

		/// <summary>
		/// Удалить контент сообщения.
		/// </summary>
		/// <param name="contentLink"></param>
		public virtual void DeleteMessageContent(int contentLink)
		{
			using ( UnitOfWork work = BeginWork() )
			{
				var dao = work.Get<DAO.MessageContentInfo>(contentLink);
				if ( dao == null )
					return;

				work.Delete(dao);
				work.End();
			}
		}
		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual List<string> GetAllRecipients()
		{
			using ( IDataQuery dataQuery = OpenQuery() )
			{
				string sql = String.Format("SELECT DISTINCT RECIPIENTS FROM {0}", Database.Tables.MESSAGES);
				ISQLQuery query = dataQuery.CreateSQLQuery(sql);
				return query.List<string>().ToList();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="begin"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public virtual List<DAO.DateStatMessage> GetMessagesByDate(string channel, DateTime? begin, DateTime? end)
		{
			using ( IDataQuery dataQuery = OpenQuery() )
			{
				var query = dataQuery.Open<DAO.DateStatMessage>();
				if ( String.IsNullOrEmpty(channel) )
					query.Where(msg => msg.Channel == null || msg.Channel == "");
				else if ( channel != "*" )
					query.Where(msg => msg.Channel == channel);

				if ( end != null )
					query.Where(msg => msg.Date <= end);

				if ( begin != null )
					query.Where(msg => msg.Date >= begin);

				return query.List().ToList();
			}
		}
		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.DbContext.ToString();
		}
		#endregion

	}
}
