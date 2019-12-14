using System;

namespace Microservices.Bus.Web.VMO
{
	/// <summary>
	/// Информация о БД.
	/// </summary>
	[Serializable]
	public class DatabaseInfo
	{

		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public DatabaseInfo()
		{ }
		#endregion


		#region Properties
		/// <summary>
		/// {Get,Set} Имя БД.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// {Get,Set} Схема БД.
		/// </summary>
		public string Schema { get; set; }

		/// <summary>
		/// {Get,Set} Провайдер.
		/// </summary>
		public string Provider { get; set; }

		/// <summary>
		/// {Get,Set} Строка подключения
		/// </summary>
		public string ConnectionString { get; set; }
		#endregion

	}
}
