using System.Collections.Generic;

namespace Microservices.Bus.Addins
{
	/// <summary>
	/// Описание дополнения.
	/// </summary>
	public interface IAddinDescription
	{
		/// <summary>
		/// Св-ва дополнения.
		/// </summary>
		IDictionary<string, AddinDescriptionProperty> Properties { get; }


		/// <summary>
		/// Провайдер.
		/// </summary>
		string Provider { get; }

		/// <summary>
		/// Имя типа/сборки/исполняемого файла.
		/// </summary>
		string Type { get; }

		/// <summary>
		/// Версия.
		/// </summary>
		string Version { get; }

		/// <summary>
		/// Комментарий.
		/// </summary>
		string Comment { get; }

		/// <summary>
		/// Имя файла иконки.
		/// </summary>
		string Icon { get; }

		/// <summary>
		/// Может обновлять список контактов.
		/// </summary>
		bool CanSyncContacts { get; }

		/// <summary>
		/// Поддержка множества экземпляров.
		/// </summary>
		bool AllowMultipleInstances { get; }

		/// <summary>
		/// Формат строки подключения.
		/// </summary>
		string RealAddress { get; }

		/// <summary>
		/// Формат строки Url адреса.
		/// </summary>
		string SID { get; }

		/// <summary>
		/// Таймаут подключения по умолчанию.
		/// </summary>
		int Timeout { get; }

		/// <summary>
		/// Путь к каталогу с исполняемыми файлами.
		/// </summary>
		string AddinPath { get; }

		/// <summary>
		/// Файл с описанием дополнения.
		/// </summary>
		string DescriptionFile { get; }

	}
}
