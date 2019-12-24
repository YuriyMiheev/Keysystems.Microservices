using System.Collections.Generic;

namespace Microservices.Bus.Addins
{
	/// <summary>
	/// Описание дополнения.
	/// </summary>
	public interface IAddinDescription
	{
		/// <summary>
		/// {Get} Св-ва дополнения.
		/// </summary>
		IDictionary<string, AddinDescriptionProperty> Properties { get; }


		/// <summary>
		/// {Get} Провайдер.
		/// </summary>
		string Provider { get; }

		/// <summary>
		/// {Get} Имя типа/сборки/исполняемого файла.
		/// </summary>
		public string Type { get; }

		/// <summary>
		/// {Get} Версия.
		/// </summary>
		public string Version { get; }

		/// <summary>
		/// {Get} Комментарий.
		/// </summary>
		public string Comment { get; }

		/// <summary>
		/// {Get} Имя файла иконки.
		/// </summary>
		public string Icon { get; }

		/// <summary>
		/// {Get} Может обновлять список контактов.
		/// </summary>
		public bool CanSyncContacts { get; }

		/// <summary>
		/// {Get} Поддержка множества экземпляров.
		/// </summary>
		public bool AllowMultipleInstances { get; }

		/// <summary>
		/// {Get} Формат строки подключения.
		/// </summary>
		public string RealAddress { get; }

		/// <summary>
		/// {Get} Формат строки Url адреса.
		/// </summary>
		public string SID { get; }

		/// <summary>
		/// {Get} Таймаут подключения по умолчанию.
		/// </summary>
		public int Timeout { get; }

		/// <summary>
		/// {Get} Путь к каталогу с исполняемыми файлами.
		/// </summary>
		public string BinPath { get; set; }

	}
}
