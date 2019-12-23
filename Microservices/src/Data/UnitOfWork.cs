using System;
using System.Data;

using NHibernate;

namespace Microservices.Data
{
	/// <summary>
	/// Еденица работы.
	/// </summary>
	/// <remarks>http://nhibernate.info/doc/nhibernate-reference/transactions.html</remarks>
	public class UnitOfWork : IDataQuery, IDisposable
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		public UnitOfWork(ISession session)
		{
			#region Validate parameters
			if ( session == null )
				throw new ArgumentNullException("session");
			#endregion

			this.Session = session;
		}

		/// <summary>
		/// Начать работу.
		/// </summary>
		/// <param name="session"></param>
		/// <param name="transaction"></param>
		public UnitOfWork(ISession session, IsolationLevel transaction)
			: this(session)
		{
			if ( transaction == IsolationLevel.Unspecified )
				this.Transaction = session.BeginTransaction();
			else
				this.Transaction = session.BeginTransaction(transaction);
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Сессия.
		/// </summary>
		public ISession Session { get; private set; }

		/// <summary>
		/// {Get} Транзакция.
		/// </summary>
		public ITransaction Transaction { get; private set; }
		#endregion


		#region Methods

		#region IDataQuery
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="link"></param>
		/// <returns></returns>
		public T Get<T>(int link) where T : class
		{
			return this.Session.Get<T>(link);
		}

		/// <summary>
		/// Открыть запрос на получение сущностей.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IQueryOver<T, T> Open<T>() where T : class
		{
			return this.Session.QueryOver<T>();
		}

		/// <summary>
		/// Открыть запрос на получение сущностей.
		/// </summary>
		/// <param name="hql"></param>
		/// <returns></returns>
		public IQuery Open(string hql)
		{
			#region Validate parameters
			if ( String.IsNullOrEmpty(hql) )
				throw new ArgumentException("hql");
			#endregion

			return this.Session.CreateQuery(hql);
		}
		#endregion


		/// <summary>
		/// Создать объект в БД.
		/// </summary>
		/// <param name="dao"></param>
		public void Save(object dao)
		{
			#region Validate parameters
			if ( dao == null )
				throw new ArgumentNullException("dao");
			#endregion

			this.Session.Save(dao);
		}

		/// <summary>
		/// Обновить объект в БД.
		/// </summary>
		/// <param name="dao"></param>
		public void Update<T>(ref T dao) where T : class
		{
			#region Validate parameters
			if ( dao == null )
				throw new ArgumentNullException("dao");
			#endregion

			dao = this.Session.Merge<T>(dao);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dao"></param>
		public void Delete(object dao)
		{
			#region Validate parameters
			if ( dao == null )
				throw new ArgumentNullException("dao");
			#endregion

			this.Session.Delete(dao);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		public ISQLQuery CreateSQLQuery(string sql)
		{
			return this.Session.CreateSQLQuery(sql);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hql"></param>
		/// <returns></returns>
		public IQuery CreateHqlQuery(string hql)
		{
			return this.Session.CreateQuery(hql);
		}

		/// <summary>
		/// 
		/// </summary>
		public void End()
		{
			//if ( this.Transaction != null && this.Transaction.IsActive && !this.Transaction.WasCommitted && !this.Transaction.WasRolledBack )
			if ( this.Transaction != null && this.Transaction.IsActive )
				this.Transaction.Commit();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Cancel()
		{
			//if ( this.Transaction != null && this.Transaction.IsActive && !this.Transaction.WasCommitted && !this.Transaction.WasRolledBack )
			if ( this.Transaction != null && this.Transaction.IsActive )
				this.Transaction.Rollback();
		}
		#endregion


		#region IDisposable
		private bool disposed;

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			if ( disposed )
				return;

			if ( this.Transaction != null )
				this.Transaction.Dispose();

			if ( this.Session != null )
			{
				using ( IDbConnection conn = this.Session.Close() )
				{ }

				this.Session.Dispose();
			}

			disposed = true;
		}
		#endregion

	}
}
