using System;
using System.Data;
using System.Data.Common;

using NHibernate;

namespace Microservices.Data
{
	/// <summary>
	/// Контекст подключения к БД.
	/// </summary>
	public class DbContext : IDisposable
	{
		private bool connectionExists;


		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="database">БД.</param>
		/// <param name="factory">Фабрика сессий.</param>
		public DbContext(IDatabase database, ISessionFactory factory)
		{
			this.Database = database ?? throw new ArgumentNullException(nameof(database));
			this.SessionFactory = factory ?? throw new ArgumentNullException(nameof(factory));
		}
		#endregion


		#region Events
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler<ConnectionChangedEventArgs> ConnectionChanged;

		private void OnConnectionChanged()
		{
			EventHandler<ConnectionChangedEventArgs> action = this.ConnectionChanged;
			if ( action != null )
			{
				var args = new ConnectionChangedEventArgs();
				args.ConnectionExists = this.connectionExists;

				action(this, args);
			}
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Ссылка на БД.
		/// </summary>
		public IDatabase Database { get; private set; }

		/// <summary>
		/// {Get} Тип провайдера.
		/// </summary>
		public string Provider
		{
			get { return this.Database.Provider; }
		}

		/// <summary>
		/// {Get} Строка подключения.
		/// </summary>
		public string ConnectionString
		{
			get { return this.Database.ConnectionString; }
		}

		/// <summary>
		/// {Get} Timeout подключения. (Default: 15 сек)
		/// </summary>
		public int ConnectionTimeout
		{
			get { return this.Database.ConnectionTimeout; }
		}

		/// <summary>
		/// {Get} Фабрика сессий.
		/// </summary>
		public ISessionFactory SessionFactory { get; private set; }
		#endregion


		#region Methods
		/// <summary>
		/// Открыть запрос.
		/// </summary>
		/// <returns></returns>
		public IDataQuery OpenQuery()
		{
			DbConnection conn = OpenConnection();
			ISession session = this.SessionFactory.WithOptions().Connection(conn).OpenSession();
			return new UnitOfWork(session);
		}

		/// <summary>
		/// Открыть запрос.
		/// </summary>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public IDataQuery OpenQuery(IsolationLevel transaction)
		{
			DbConnection conn = OpenConnection();
			ISession session = this.SessionFactory.WithOptions().Connection(conn).OpenSession();
			return new UnitOfWork(session, transaction);
		}

		/// <summary>
		/// Начать работу.
		/// </summary>
		/// <returns></returns>
		public UnitOfWork BeginWork()
		{
			DbConnection conn = OpenConnection();
			ISession session = this.SessionFactory.WithOptions().Connection(conn).OpenSession();
			return new UnitOfWork(session, IsolationLevel.Unspecified);
		}

		/// <summary>
		/// Начать работу.
		/// </summary>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public UnitOfWork BeginWork(IsolationLevel transaction)
		{
			DbConnection conn = OpenConnection();
			ISession session = this.SessionFactory.WithOptions().Connection(conn).OpenSession();
			return new UnitOfWork(session, transaction);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Database.ToString();
		}
		#endregion


		#region Helpers
		private DbConnection OpenConnection()
		{
			try
			{
				DbConnection conn = this.Database.OpenNewConnection();

				if ( !this.connectionExists )
				{
					this.connectionExists = true;
					OnConnectionChanged();
				}

				return conn;
			}
			catch ( ConnectionException )
			{
				if ( this.connectionExists )
				{
					this.connectionExists = false;
					OnConnectionChanged();
				}

				throw;
			}
		}
		#endregion


		#region IDisposable
		private bool disposed = false;

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if ( disposed )
				return;

			lock ( this )
			{
				if ( disposed )
					return;

				if ( disposing )
				{
					this.ConnectionChanged = null;
					this.SessionFactory.Dispose();
				}

				disposed = true;
			}
		}

		/// <summary>
		/// Деструктор.
		/// </summary>
		~DbContext()
		{
			Dispose(false);
		}
		#endregion

	}
}
