using System;
using System.IO;
using System.Linq;

namespace Microservices.Channels.MSSQL.Configuration
{
	/// <summary>
	/// Конфигурация сервиса.
	/// </summary>
	public interface IServiceConfig
	{

		#region Properties
		/// <summary>
		/// {Get} Признак завершения процедуры конфигурирования.
		/// </summary>
		bool IsConfigured { get; }

		///// <summary>
		///// {Get} Настройки.
		///// </summary>
		//IConfigFileSettings ConfigFileSettings { get; }

		///// <summary>
		///// {Get} Логер.
		///// </summary>
		//ILogger Logger { get; }

		///// <summary>
		///// {Get} Кэш.
		///// </summary>
		//IServiceCache Cache { get; }

		/// <summary>
		/// {Get}
		/// </summary>
		string TempDir { get; }
		#endregion


		#region Methods
		IServiceConfig CreateWorkingDirectories();

		///// <summary>
		///// 
		///// </summary>
		///// <param name="logger"></param>
		///// <returns></returns>
		//IServiceConfig DefineLogger(ILogger logger);

		///// <summary>
		///// 
		///// </summary>
		///// <param name="cache"></param>
		///// <returns></returns>
		//IServiceConfig DefineCache(IServiceCache cache);

		/// <summary>
		/// 
		/// </summary>
		void EndConfigure();
		#endregion

	}
}
