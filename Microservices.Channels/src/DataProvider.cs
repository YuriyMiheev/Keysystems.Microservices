using System;
using System.Linq;

namespace Microservices.Channels
{
	/// <summary>
	/// Тип провайдера.
	/// </summary>
	public class DataProvider
	{
		/// <summary>
		/// Система.
		/// </summary>
		public const string SYSTEM = "_SYSTEM_";

		/// <summary>
		/// Удаленный RMS-сервис.
		/// </summary>
		public const string REMOTE = "REMOTE";

		/// <summary>
		/// СМЭВ.
		/// </summary>
		public const string SMEV = "SMEV";

		/// <summary>
		/// СИР.
		/// </summary>
		public const string SIR = "SIR";

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
		/// Файловый каталог.
		/// </summary>
		public const string FILE = "FILE";

		/// <summary>
		/// Передача файлов.
		/// </summary>
		public const string FILE_TRANSFER = "FILE_TRANSFER";

		/// <summary>
		/// Внешний Web-сервис.
		/// </summary>
		public const string WS_PROXY = "WS-PROXY";

		///// <summary>
		///// Брокер сообщений RabbitMQ.
		///// </summary>
		//public const string RABBITMQ = "RABBITMQ";

		///// <summary>
		///// 
		///// </summary>
		//public const string FTP = "FTP";

		///// <summary>
		///// Почта.
		///// </summary>
		//public const string MAIL = "MAIL";

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
