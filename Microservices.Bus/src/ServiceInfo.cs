using System;

using Microservices.Configuration;

namespace Microservices.Bus
{
	/// <summary>
	/// Информация о сервисе.
	/// </summary>
	public class ServiceInfo
	{

		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public int LINK { get; set; }

		/// <summary>
		/// {Get} Идентификатор экземпляра сервиса сообщений.
		/// </summary>
		public string InstanceID { get; set; }

		/// <summary>
		/// {Get} Имя сервиса.
		/// </summary>
		public string ServiceName { get; set; }

		/// <summary>
		/// {Get} Имя компьютера.
		/// </summary>
		public string MachineName { get; set; }

		/// <summary>
		/// {Get} Версия сервиса.
		/// </summary>
		public string Version { get; set; }

		private DateTime? _currentTime;
		/// <summary>
		/// {Get} Текущее время на сервисе.
		/// </summary>
		public DateTime CurrentTime
		{
			get { return (_currentTime ?? DateTime.Now); }
			set { _currentTime = value; }
		}

		/// <summary>
		/// {Get} Время запуска сервиса.
		/// </summary>
		public DateTime StartTime { get; set; }

		/// <summary>
		/// {Get} Время остановки сервиса.
		/// </summary>
		public DateTime? ShutdownTime { get; set; }

		/// <summary>
		/// {Get} Причина остановки.
		/// </summary>
		public string ShutdownReason { get; set; }

		/// <summary>
		/// {Get} Сервис запущен.
		/// </summary>
		public bool Running { get; set; }

		/// <summary>
		/// {Get} Сервис доступен.
		/// </summary>
		public bool Online { get; set; }

		/// <summary>
		/// {Get} Имя конфигурационного файла.
		/// </summary>
		public string ConfigFileName { get; set; }

		/// <summary>
		/// {Get} Базовый каталог.
		/// </summary>
		public string BaseDir { get; set; }

		/// <summary>
		/// {Get} Путь к каталогу лог файлов.
		/// </summary>
		public string LogFilesDir { get; set; }

		/// <summary>
		/// {Get} Путь к временному каталогу.
		/// </summary>
		public string TempDir { get; set; }

		/// <summary>
		/// {Get} Путь к каталогу дополнений.
		/// </summary>
		public string AddinsDir { get; set; }

		/// <summary>
		/// {Get} Путь к каталогу утилит.
		/// </summary>
		public string ToolsDir { get; set; }

		/// <summary>
		/// {Get} Путь к файлу лицензий.
		/// </summary>
		public string LicenseFile { get; set; }

		/// <summary>
		/// {Get} Служебная БД сервиса.
		/// </summary>
		public DatabaseInfo Database { get; set; }

		/// <summary>
		/// {Get} Данные администратора.
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
		/// {Get} Максимальный загружаемый на сервис размер файла.
		/// </summary>
		public int MaxUploadSize { get; set; }

		/// <summary>
		/// {Get,Set} Ошибка при запуске.
		/// </summary>
		public Exception StartupError { get; set; }
	}
}
