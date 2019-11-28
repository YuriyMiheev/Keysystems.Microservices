using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mime;
using System.Text;

using NHibernate;
using NHibernate.Criterion;

using Microservices.Channels.Adapters;
using Microservices.Channels.Data;
using Microservices.Channels.MSSQL.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace Microservices.Channels.MSSQL.Adapters
{
	/// <summary>
	/// Базовый класс для доступа к БД.
	/// </summary>
	public class MessageDataAdapter : MessageDataAdapterBase, IMessageDataAdapter
	{

		#region Ctor
		public MessageDataAdapter(IDatabase database)
			: base(database)
		{ }

		/// <summary>
		/// Создание экземпляра.
		/// </summary>
		/// <param name="dbContext"></param>
		public MessageDataAdapter(DbContext dbContext)
			: base(dbContext)
		{ }
		#endregion


		protected override MessageBodyStreamBase ConstructMessageBodyStream(int msgLink, UnitOfWork work, DataStreamMode mode, Encoding encoding)
		{
			return new MessageBodyStream(this.DbContext, work, mode, msgLink, encoding);
		}

		protected override MessageContentStreamBase ConstructMessageContentStream(int contentLink, UnitOfWork work, DataStreamMode mode, Encoding encoding)
		{
			return new MessageContentStream(this.DbContext, work, mode, contentLink, encoding);
		}


		#region Contacts
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<Contact> GetContacts()
		{
			using (IDataQuery dataQuery = OpenQuery())
			{
				return dataQuery.Open<DAO.Contact>().List().Select(dao => dao.ToObj()).ToList();
			}
		}

		/// <summary>
		/// Сохранить контакт.
		/// </summary>
		/// <param name="contact"></param>
		public void SaveContact(Contact contact)
		{
			#region Validate parameters
			if (contact == null)
				throw new ArgumentNullException("contact");
			#endregion

			DAO.Contact dao = contact.ToDao();

			using (UnitOfWork work = BeginWork())
			{
				if (contact.LINK == 0)
					work.Save(dao);
				else
					work.Update<DAO.Contact>(ref dao);

				work.End();
			}

			dao.CloneTo(contact);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contact"></param>
		public void DeleteContact(Contact contact)
		{
			#region Validate parameters
			if (contact == null)
				throw new ArgumentNullException("contact");
			#endregion

			DAO.Contact dao = contact.ToDao();

			using (UnitOfWork work = BeginWork())
			{
				work.Delete(dao);
				work.End();
			}
		}
		#endregion


		#region StoredProcedure
		/// <summary>
		/// Выполнить хранимую процедуру пинга БД.
		/// </summary>
		/// <param name="spName"></param>
		public void CallPingSP(string spName)
		{
			#region Validate parameters
			if (String.IsNullOrEmpty(spName))
				throw new ArgumentException("Не указано имя хранимой процедуры.", "spName");
			#endregion

			string sql = String.Format("EXEC {0}", spName);

			try
			{
				using (UnitOfWork work = BeginWork())
				{
					ISQLQuery sqlQuery = work.CreateSQLQuery(sql);
					sqlQuery.SetTimeout(this.ExecuteTimeout);

					int res = sqlQuery.ExecuteUpdate();

					work.End();
				}
			}
			catch (Exception ex)
			{
				throw CatchStoredProcedureException(spName, ex);
			}
		}

		/// <summary>
		/// Выполнить хранимую процедуру восстановления БД.
		/// </summary>
		/// <param name="spName"></param>
		public void CallRepairSP(string spName)
		{
			#region Validate parameters
			if (String.IsNullOrEmpty(spName))
				throw new ArgumentException("Не указано имя хранимой процедуры.", "spName");
			#endregion

			string sql = String.Format("EXEC {0}", spName);

			try
			{
				using (UnitOfWork work = BeginWork())
				{
					ISQLQuery sqlQuery = work.CreateSQLQuery(sql);
					sqlQuery.SetTimeout(this.ExecuteTimeout);

					int res = sqlQuery.ExecuteUpdate();

					work.End();
				}
			}
			catch (Exception ex)
			{
				throw CatchStoredProcedureException(spName, ex);
			}
		}

		/// <summary>
		/// Выполнить хранимую процедуру уведомления изменения статуса сообщения.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="msg"></param>
		public void CallMessageStatusChangedSP(string spName, Message msg)
		{
			#region Validate parameters
			if (String.IsNullOrEmpty(spName))
				throw new ArgumentException("Не указано имя хранимой процедуры.", "spName");

			if (msg == null)
				throw new ArgumentNullException("msg");
			#endregion

			string sql = String.Format("EXEC {0} :msgLink, :prevStatus, :newStatus", spName);

			try
			{
				using (UnitOfWork work = BeginWork())
				{
					ISQLQuery sqlQuery = work.CreateSQLQuery(sql);
					sqlQuery.SetTimeout(this.ExecuteTimeout);
					sqlQuery.SetParameter("msgLink", msg.LINK);
					sqlQuery.SetParameter("prevStatus", msg.PrevStatus.Value);
					sqlQuery.SetParameter("newStatus", msg.Status.Value);

					int res = sqlQuery.ExecuteUpdate();

					work.End();
				}
			}
			catch (Exception ex)
			{
				throw CatchStoredProcedureException(spName, ex);
			}
		}

		/// <summary>
		/// Выполнить хранимую процедуру приема сообщения.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="msg"></param>
		/// <param name="useOutputParam"></param>
		/// <returns>resLink</returns>
		public int? CallReceiveMessageSP(string spName, Message msg, bool useOutputParam)
		{
			#region Validate parameters
			if (String.IsNullOrEmpty(spName))
				throw new ArgumentException("Не указано имя хранимой процедуры.", "spName");

			if (msg == null)
				throw new ArgumentNullException("msg");
			#endregion

			try
			{
				using (UnitOfWork work = BeginWork())
				{
					DbCommand command = new SqlCommand();
					SqlParameter param;

					if (useOutputParam)
						param = new SqlParameter("@msgLink", msg.LINK) { Direction = ParameterDirection.InputOutput };
					else
						param = new SqlParameter("@msgLink", msg.LINK) { Direction = ParameterDirection.Input };

					using (command as IDisposable)
					{
						work.Transaction.Enlist(command);

						command.Connection = work.Session.Connection;
						command.CommandTimeout = this.ExecuteTimeout;
						command.CommandType = CommandType.StoredProcedure;
						command.CommandText = spName;
						command.Parameters.Add(param);

						int res = command.ExecuteNonQuery();
					}

					work.End();

					if (useOutputParam)
					{
						if (param.Value is DBNull)
							return null;
						else
							return (int)param.Value;
					}
					else
					{
						return null;
					}
				}
			}
			catch (Exception ex)
			{
				throw CatchStoredProcedureException(spName, ex);
			}
		}

		private Exception CatchStoredProcedureException(string spName, Exception ex)
		{
			SqlException sqlEx = ex.Find<SqlException>();
			if (sqlEx != null)
			{
				string error = "";
				foreach (SqlError sqlError in sqlEx.Errors)
				{
					error += String.Format("Procedure: \"{0}\", LineNumber: {1}, State: {2}, Number: {3}, Class: {4}, Message: \"{5}\"", sqlError.Procedure, sqlError.LineNumber, sqlError.State, sqlError.Number, sqlError.Class, (sqlError.Message ?? "").Trim(' ', '\r', '\n')) + Environment.NewLine;
				}

				return new StoredProcedureException(error, ex);
			}

			return ex;
		}
		#endregion

	}
}
