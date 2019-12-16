using System;
using System.Collections.Generic;

namespace Microservices.Bus.Web.VMO
{
	/// <summary>
	/// Информация о сервисе.
	/// </summary>
	public class ServiceInfo
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public ServiceInfo()
		{ }
		#endregion


		#region Properties
		/// <summary>
		/// {Get,Set} Внутренний ID.
		/// </summary>
		public int LINK { get; set; }

		/// <summary>
		/// {Get,Set} Идентификатор экземпляра сервиса сообщений.
		/// </summary>
		public string InstanceID { get; set; }

		/// <summary>
		/// {Get,Set} Имя сервиса.
		/// </summary>
		public string ServiceName { get; set; }

		/// <summary>
		/// {Get,Set} Имя компьютера.
		/// </summary>
		public string MachineName { get; set; }

		/// <summary>
		/// {Get,Set} Версия сервиса.
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		/// {Get,Set} Текущее время на сервисе.
		/// </summary>
		public DateTime CurrentTime { get; set; }

		/// <summary>
		/// {Get,Set} Время запуска сервиса.
		/// </summary>
		public DateTime StartTime { get; set; }

		/// <summary>
		/// {Get,Set} Время остановки сервиса.
		/// </summary>
		public DateTime? ShutdownTime { get; set; }

		/// <summary>
		/// {Get,Set} Причина остановки.
		/// </summary>
		public string ShutdownReason { get; set; }

		/// <summary>
		/// {Get,Set} Сервис запущен.
		/// </summary>
		public bool Running { get; set; }

		/// <summary>
		/// {Get,Set} Сервис доступен.
		/// </summary>
		public bool Online { get; set; }

		/// <summary>
		/// {Get,Set} Имя конфигурационного файла.
		/// </summary>
		public string ConfigFileName { get; set; }

		/// <summary>
		/// {Get,Set} Базовый каталог.
		/// </summary>
		public string BaseDir { get; set; }

		/// <summary>
		/// {Get,Set} Путь к каталогу лог файлов.
		/// </summary>
		public string LogFilesDir { get; set; }

		/// <summary>
		/// {Get,Set} Путь к временному каталогу.
		/// </summary>
		public string TempDir { get; set; }

		/// <summary>
		/// {Get} Путь к каталогу дополнений.
		/// </summary>
		public string AddinsDir { get; set; }

		/// <summary>
		/// {Get,Set} Путь к каталогу утилит.
		/// </summary>
		public string ToolsDir { get; set; }

		/// <summary>
		/// {Get,Set} Путь к файлу лицензий.
		/// </summary>
		public string LicenseFile { get; set; }

		/// <summary>
		/// {Get,Set} Служебная БД сервиса.
		/// </summary>
		public DatabaseInfo Database { get; set; }

		/// <summary>
		/// {Get,Set} Данные администратора.
		/// </summary>
		public CredentialInfo Administrator { get; set; }

		/// <summary>
		/// {Get,Set} Внешний URL сервиса.
		/// </summary>
		public string ExternalAddress { get; set; }

		/// <summary>
		/// {Get,Set} Внутренний URL сервиса.
		/// </summary>
		public string InternalAddress { get; set; }

		/// <summary>
		/// {Get,Set} Отладка включена.
		/// </summary>
		public bool DebugEnabled { get; set; }

		/// <summary>
		/// {Get,Set} Авторизация включена.
		/// </summary>
		public bool AuthorizeEnabled { get; set; }

		/// <summary>
		/// {Get,Set} Максимальный загружаемый на сервис размер файла.
		/// </summary>
		public int MaxUploadSize { get; set; }

		/// <summary>
		/// {Get,Set} Ошибка при запуске.
		/// </summary>
		public ExceptionWrapper StartupError { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public List<ServiceProperty> Properties { get; set; }
		#endregion


		//#region Methods
		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//public ServiceInfo Clear()
		//{
		//	if ( this.Properties != null )
		//		this.Properties.Clear();

		//	return this;
		//}
		//#endregion



	}
}
