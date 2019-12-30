using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Bus.Addins;
using Microservices.Bus.Authorization;
using Microservices.Bus.Channels;
using Microservices.Bus.Configuration;
using Microservices.Bus.Data;
using Microservices.Bus.Licensing;
using Microservices.Bus.Logging;
using Microservices.Configuration;
using Microservices.Data;

using Microsoft.Extensions.DependencyInjection;

using DAO = Microservices.Bus.Data.DAO;

namespace Microservices.Bus
{
	public class BusService : IBusService
	{
		private CancellationTokenSource _cancellationSource;
		private readonly IAppSettingsConfig _appConfig;
		private readonly IConnectionStringsConfig _connConfig;
		private readonly BusSettings _busSettings;
		private readonly ILogger _logger;
		private readonly IBusDatabase _database;
		private readonly IBusDataAdapter _dataAdapter;
		private readonly IAuthManager _authManager;
		private readonly IChannelManager _channelManager;
		private readonly IAddinManager _addinManager;
		private readonly ILicenseManager _licManager;
		private readonly ServiceInfo _serviceInfo;


		#region Ctor
		public BusService(IServiceProvider serviceProvider)
		{
			_cancellationSource = new CancellationTokenSource();

			_appConfig = serviceProvider.GetRequiredService<IAppSettingsConfig>();
			_connConfig = serviceProvider.GetRequiredService<IConnectionStringsConfig>();
			_busSettings = serviceProvider.GetRequiredService<BusSettings>();
			_logger = serviceProvider.GetRequiredService<ILogger>();
			_database = serviceProvider.GetRequiredService<IBusDatabase>();
			_dataAdapter = serviceProvider.GetRequiredService<IBusDataAdapter>();
			_authManager = serviceProvider.GetRequiredService<IAuthManager>();
			_channelManager = serviceProvider.GetRequiredService<IChannelManager>();
			_addinManager = serviceProvider.GetRequiredService<IAddinManager>();
			_licManager = serviceProvider.GetRequiredService<ILicenseManager>();
			_serviceInfo = serviceProvider.GetRequiredService<ServiceInfo>();
			//_serviceInfoManager = serviceProvider.GetRequiredService<IServiceInfoManager>();

			SetCurrentParamsTo(_serviceInfo);
		}
		#endregion


		///// <summary>
		///// 
		///// </summary>
		//public ServiceInfo GetInfo()
		//{
		//	var serviceInfo = new ServiceInfo();
		//	_serviceInfo.CloneTo(serviceInfo);
		//	//serviceInfo.SortPropertiesByName();
		//	return serviceInfo;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="updateParams"></param>
		//public void UpdateInfo(ServiceInfoUpdateParams updateParams)
		//{
		//	_serviceInfo.Online = updateParams.Online;
		//	_serviceInfo.InternalAddress = updateParams.InternalAddress;
		//	_serviceInfo.ExternalAddress = updateParams.ExternalAddress;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="reinitService"></param>
		//public void SaveInfo(bool reinitService)
		//{
		//}


		#region IHostedService
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogTrace("Старт сервиса.");

			bool isSchemaValid = false;

			try
			{
				_serviceInfo.StartTime = DateTime.Now;

				_database.Schema = _busSettings.Database.Schema;
				_database.ConnectionString = _busSettings.Database.ConnectionString;
				//_database.ConnectionTimeout = (int)_databaseSettings.ConnectionTimeout.TotalSeconds;
				//_dataAdapter.ExecuteTimeout = (int)_databaseSettings.ExecuteTimeout.TotalSeconds;

				while (!_database.TryConnect(out ConnectionException error))
				{
					_serviceInfo.StartupError = error;
					System.Threading.Thread.Sleep(1000);
				}

				using DbContext dbContext = _database.ValidateSchema();
				//using DbContext dbContext = _database.CreateOrUpdateSchema();
				isSchemaValid = true;

				List<DAO.ServiceInfo> instances = _dataAdapter.GetServiceInstances();
				if (instances.Count > 0)
				{
					DAO.ServiceInfo service = instances[0];
					_serviceInfo.LINK = service.LINK;
					if (service.InstanceID != _serviceInfo.InstanceID && _busSettings.CheckInstanceID)
						throw new ApplicationException("База данных принадлежит другому интеграционному сервису.");
				}

				_addinManager.LoadAddins();

				//_licManager.LoadLicenses();
				await _channelManager.LoadChannelsAsync(cancellationToken);

				_serviceInfo.StartupError = null;
				_serviceInfo.Running = true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				_serviceInfo.StartupError = ex;
			}
			finally
			{
				if (isSchemaValid)
					_dataAdapter.SaveServiceInfo(_serviceInfo);
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.Run(() =>
				{
					_logger.LogTrace("Остановка сервиса.");
					_serviceInfo.ShutdownTime = DateTime.Now;
					_database.Close();
				}, cancellationToken);
		}
		#endregion


		#region Helpers
		private void SetCurrentParamsTo(ServiceInfo serviceInfo)
		{
			serviceInfo.InstanceID = _busSettings.InstanceID;
			serviceInfo.ServiceName = "Integration Service Bus";
			serviceInfo.MachineName = Environment.MachineName;
			serviceInfo.Version = "1.0"; //MessageServiceVersion.Current.Version;
			serviceInfo.ConfigFileName = _appConfig.ConfigFile;
			serviceInfo.BaseDir = AppDomain.CurrentDomain.BaseDirectory;
			//serviceInfo.LogFilesDir = _busSettings.LogFilesDir;
			serviceInfo.TempDir = _busSettings.TempDir;
			serviceInfo.AddinsDir = _busSettings.AddinsDir;
			serviceInfo.ToolsDir = _busSettings.ToolsDir;
			serviceInfo.LicenseFile = _busSettings.LicenseFile;
			serviceInfo.Database = _busSettings.Database;
			serviceInfo.Administrator = _busSettings.Administrator;
			serviceInfo.DebugEnabled = _busSettings.DebugEnabled;
			serviceInfo.AuthorizeEnabled = _busSettings.AuthorizationRequired;
			serviceInfo.MaxUploadSize = _busSettings.MaxUploadFileSize;
		}
		#endregion

	}
}
