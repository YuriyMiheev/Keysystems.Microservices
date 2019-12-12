using System;

namespace Microservices.Configuration
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


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if ( String.IsNullOrWhiteSpace(this.Schema) )
				return String.Format("name=\"{0}\" providerName=\"{1}\" connectionString=\"{2}\"", this.Name, this.Provider, this.ConnectionString);
			else
				return String.Format("name=\"[{0}].{1}\" providerName=\"{2}\" connectionString=\"{3}\"", this.Schema, this.Name, this.Provider, this.ConnectionString);
		}
		#endregion

	}
}
