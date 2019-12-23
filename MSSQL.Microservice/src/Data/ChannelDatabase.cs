using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using Microservices;
using Microservices.Data;
using Microservices.Data.Mappings;
using Microservices.Data.MSSQL;

using NHibernate.Tool.hbm2ddl;

using NH = NHibernate;

namespace MSSQL.Microservice.Data
{
	/// <summary>
	/// БД сообщений.
	/// </summary>
	public class ChannelDatabase : IDatabase, IDisposable
	{
		/// <summary>
		/// "dbo"
		/// </summary>
		public static readonly string DefaultSchema = "dbo";


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public ChannelDatabase()
		{
			this.ConnectionTimeout = 15;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Тип провайдера.
		/// </summary>
		public string Provider
		{
			get { return DataProvider.MSSQL; }
		}

		/// <summary>
		/// {Get, Set} Схема БД.
		/// </summary>
		public string Schema { get; set; }

		private string connString;
		/// <summary>
		/// {Get,Set} Строка подключения.
		/// </summary>
		public string ConnectionString
		{
			get { return connString; }
			set
			{
				if (String.IsNullOrEmpty(value))
					throw new ArgumentException("Пустое значение.", "value");

				connString = value;
			}
		}

		/// <summary>
		/// {Get,Set} Timeout подключения. (Default: 15 сек)
		/// </summary>
		public int ConnectionTimeout { get; set; }
		#endregion


		#region Methods
		/// <summary>
		/// Проверить подключение к БД.
		/// </summary>
		public bool TryConnect(out ConnectionException error)
		{
			error = null;

			try
			{
				using (DbConnection conn = OpenNewConnection())
				{
					return true;
				}
			}
			catch (ConnectionException ex)
			{
				error = ex;
				return false;
			}
		}

		/// <summary>
		/// Открыть новое подключение к БД.
		/// </summary>
		/// <returns></returns>
		public virtual DbConnection OpenNewConnection()
		{
			try
			{
				var builder = new SqlConnectionStringBuilder(this.ConnectionString);
				builder.ConnectTimeout = this.ConnectionTimeout;
				var conn = new SqlConnection(builder.ConnectionString);
				conn.Open();
				return conn;
			}
			catch (Exception ex)
			{
				//System.ComponentModel.Win32Exception
				throw new ConnectionException($"Ошибка подключения к БД: {this}.", ex);
			}
		}

		/// <summary>
		/// Открыть БД.
		/// </summary>
		/// <returns></returns>
		public virtual DbContext Open()
		{
			try
			{
				FluentConfiguration dbConfig = Configure();
				return new DbContext(this, dbConfig.BuildSessionFactory());
			}
			catch (Exception ex)
			{
				throw new DatabaseException($"Ошибка открытия БД: {this}.", ex);
			}
		}

		/// <summary>
		/// Закрыть подключение к БД.
		/// </summary>
		public virtual void Close()
		{
			Dispose();
			//try
			//{
			//}
			//catch (Exception ex)
			//{
			//	Trace.TraceError(ex.ToString());
			//}
			//finally
			//{
			//}
		}

		/// <summary>
		/// Пооверить структуру таблиц.
		/// </summary>
		/// <returns></returns>
		public virtual DbContext ValidateSchema()
		{
			try
			{
				FluentConfiguration dbConfig = Configure();
				NH.Cfg.Configuration config = dbConfig.BuildConfiguration();
				var schema = new SchemaValidator(config);
				schema.Validate();

				return new DbContext(this, dbConfig.BuildSessionFactory());
			}
			catch (Exception ex)
			{
				throw new DatabaseException($"Некорректная структура таблиц БД: {this}.", ex);
			}
		}

		/// <summary>
		/// Создать/Обновить структуру таблиц.
		/// </summary>
		/// <returns></returns>
		public virtual DbContext CreateOrUpdateSchema()
		{
			try
			{
				FluentConfiguration dbConfig = Configure();
				dbConfig.ExposeConfiguration(config =>
					{
						var schema = new SchemaUpdate(config);
						schema.Execute(false, true);
					});

				return new DbContext(this, dbConfig.BuildSessionFactory());
			}
			catch (Exception ex)
			{
				throw new DatabaseException($"Ошибка создания/обновления схемы БД: {this}.", ex);
			}
		}

		/// <summary>
		/// Пересоздать структуру таблиц (данные будут потеряны).
		/// </summary>
		/// <returns></returns>
		public virtual DbContext RecreateSchema()
		{
			try
			{
				FluentConfiguration dbConfig = Configure();
				dbConfig.ExposeConfiguration(config =>
					{
						var schema = new SchemaExport(config);
						schema.Create(false, true);
					});

				return new DbContext(this, dbConfig.BuildSessionFactory());
			}
			catch (Exception ex)
			{
				throw new DatabaseException($"Ошибка пересоздания схемы БД: {this}.", ex);
			}
		}

		/// <summary>
		/// Конфигуратор хранилища.
		/// </summary>
		/// <returns></returns>
		protected virtual IPersistenceConfigurer GetPersistenceConfigurer()
		{
			var builder = new SqlConnectionStringBuilder(this.ConnectionString);
			builder.ConnectTimeout = this.ConnectionTimeout;

			MsSqlConfiguration config = MsSqlConfiguration.MsSql2005.ConnectionString(builder.ConnectionString);

			if (!String.IsNullOrWhiteSpace(this.Schema))
				config.DefaultSchema(this.Schema);

			return config;
		}

		private List<Type> GetMappings()
		{
			var mappings = new List<Type>();
			mappings.Add(typeof(MessageMapping));
			mappings.Add(typeof(MessageBodyMapping));
			mappings.Add(typeof(MessageBodyInfoMapping));
			mappings.Add(typeof(MessageBodyInfoComponentMapping));
			mappings.Add(typeof(MessageStatusMapping));
			mappings.Add(typeof(MessagePropertyMapping));
			mappings.Add(typeof(MessageContentMapping));
			mappings.Add(typeof(MessageContentInfoMapping));
			//mappings.Add(typeof(MessagePostMapping));
			mappings.Add(typeof(DateStatMessageMapping));
			mappings.Add(typeof(ContactMapping));
			mappings.Add(typeof(ContactPropertyMapping));
			return mappings;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return DatabaseString(this.Provider, this.ConnectionString);
		}
		#endregion


		#region Helpers
		private FluentConfiguration Configure()
		{
			IPersistenceConfigurer peristentType = GetPersistenceConfigurer();
			List<Type> mappings = GetMappings();

			FluentConfiguration config = Fluently.Configure()
				.Database(peristentType)
				.Mappings(map => mappings.ForEach(type => map.FluentMappings.Add(type)))
				.ExposeConfiguration(cfg => { cfg.SetProperty(NH.Cfg.Environment.CommandTimeout, this.ConnectionTimeout.ToString()); });

			return config;
		}
		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		public static string DatabaseString(string provider, string connectionString)
		{
			var builder = new SqlConnectionStringBuilder(connectionString);
			return $"Provider={provider}, Server={builder.DataSource}, Database={builder.InitialCatalog}";
		}


		#region IDisposable
		private bool _disposed = false;

		/// <summary>
		/// Разрушить объект и освободить занятые им ресурсы.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Разрушить объект и освободить занятые им ресурсы.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
			}

			_disposed = true;
		}

		/// <summary>
		/// Деструктор.
		/// </summary>
		~ChannelDatabase()
		{
			Dispose(false);
		}
		#endregion

	}
}
