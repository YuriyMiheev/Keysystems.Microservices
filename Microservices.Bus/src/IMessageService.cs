
namespace Microservices.Bus
{
	/// <summary>
	/// Сервис сообщений.
	/// </summary>
	public interface IMessageService
	{

		#region Managers
		///// <summary>
		///// {Get}
		///// </summary>
		//IChannelManager ChannelManager { get; }

		///// <summary>
		///// {Get}
		///// </summary>
		//IJobManager JobManager { get; }

		///// <summary>
		///// {Get}
		///// </summary>
		//IAuthManager AuthManager { get; }

		///// <summary>
		///// {Get}
		///// </summary>
		//IAddinManager AddinManager { get; }

		///// <summary>
		///// {Get}
		///// </summary>
		//IPluginManager PluginManager { get; }

		///// <summary>
		///// {Get}
		///// </summary>
		//ILicenseManager LicenseManager { get; }

		///// <summary>
		///// {Get}
		///// </summary>
		//IFileManager FileManager { get; }

		///// <summary>
		///// {Get}
		///// </summary>
		//IServiceConfig Config { get; }

		///// <summary>
		///// {Get}
		///// </summary>
		//IServiceCache Cache { get; }

		///// <summary>
		///// {Get}
		///// </summary>
		//ILogger Logger { get; }
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		SysDatabase Database { get; }

		/// <summary>
		/// {Get}
		/// </summary>
		SysDataAdapter DataAdapter { get; }
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		void Start();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reason"></param>
		void Stop(string reason);

		/// <summary>
		/// 
		/// </summary>
		ServiceInfo GetInfo();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="updateParams"></param>
		void UpdateInfo(ServiceInfoUpdateParams updateParams);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reinitService"></param>
		void SaveInfo(bool reinitService);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		ServiceExpImpInfo GetExportSettings();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="import"></param>
		void ImportSettings(ServiceExpImpInfo import);
		#endregion

	}
}
