using System;
using System.Linq;

namespace Microservices.Data
{
	/// <summary>
	/// Тип провайдера.
	/// </summary>
	public class DataProvider
	{
		/// <summary>
		/// БД MSSQL.
		/// </summary>
		public const string MSSQL = "MSSQL";

		/// <summary>
		/// БД ORACLE.
		/// </summary>
		public const string ORACLE = "ORACLE";

		/// <summary>
		/// БД SQLite.
		/// </summary>
		public const string SQLITE = "SQLITE";

		/// <summary>
		/// БД MYSQL.
		/// </summary>
		public const string MYSQL = "MYSQL";

		/// <summary>
		/// БД PostgreSQL.
		/// </summary>
		public const string POSTGRESQL = "POSTGRESQL";



		/// <summary>
		/// БД провайдеры.
		/// </summary>
		public static string[] DbProviders
		{
			get { return new string[] { MSSQL, ORACLE, SQLITE, MYSQL, POSTGRESQL }; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public static bool IsDatabase(string provider)
		{
			return DbProviders.Contains(provider);
		}
	}
}
