using System;
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

namespace Microservices.Bus
{
	public class MessageService : IMessageService
	{
		private CancellationTokenSource _cancellationSource;
		private readonly IAppSettingsConfig _appConfig;
		private readonly IConnectionStringsConfig _connConfig;
		private readonly BusSettings _busSettings;
		private readonly ILogger _logger;
		private readonly IDatabase _database;
		private readonly IBusDataAdapter _dataAdapter;
		private readonly IAuthManager _authManager;
		private readonly IChannelManager _channelManager;
		private readonly IAddinManager _addinManager;
		private readonly ILicenseManager _licManager;
		private readonly ServiceInfo _serviceInfo;
		//private readonly IServiceInfoManager _serviceInfoManager;
		//private DateTime? _startTime;
		//private DateTime? _shutdownTime;
		//private Exception _startupError;
		//private bool _started;


		#region Ctor
		public MessageService(IServiceProvider serviceProvider)
		{
			_cancellationSource = new CancellationTokenSource();

			_appConfig = serviceProvider.GetRequiredService<IAppSettingsConfig>();
			_connConfig = serviceProvider.GetRequiredService<IConnectionStringsConfig>();
			_busSettings = serviceProvider.GetRequiredService<BusSettings>();
			_logger = serviceProvider.GetRequiredService<ILogger>();
			_database = serviceProvider.GetRequiredService<IDatabase>();
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


		#region IHostedService
		public Task StartAsync(CancellationToken cancellationToken)
		{
			return Task.Run(() =>
				{
					_logger.LogTrace("Старт сервиса.");

					try
					{
						_serviceInfo.StartTime = DateTime.Now;

						_database.Schema = _busSettings.Database.Schema;
						_database.ConnectionString = _busSettings.Database.ConnectionString;
						//_dataAdapter.ExecuteTimeout = (int)_databaseSettings.ExecuteTimeout.TotalSeconds;

						while (!_database.TryConnect(out ConnectionException error))
						{
							_serviceInfo.StartupError = error;
							System.Threading.Thread.Sleep(1000);
						}

						using DbContext dbContext = _database.ValidateSchema();
						//using DbContext dbContext = _database.CreateOrUpdateSchema();

						try
						{
							_addinManager.LoadAddins();
						}
						catch (Exception ex)
						{
							throw new InvalidOperationException("Ошибка загрузки дополнений.", ex);
						}

						//_licManager.LoadLicenses();
						//_channelManager.LoadChannels();

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
						//SetCurrentParamsTo(_serviceInfo);
						//_dataAdapter.SaveServiceInfo(false);
					}
				}, cancellationToken);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.Run(() =>
				{
					_logger.LogTrace("Остановка сервиса.");
					_serviceInfo.ShutdownTime = DateTime.Now;
				});
		}
		#endregion


		#region Helpers
		private void SetCurrentParamsTo(ServiceInfo serviceInfo)
		{
			serviceInfo.InstanceID = Guid.NewGuid().ToString(); //this.instanceId;
			serviceInfo.ServiceName = "Integration Service Bus";
			serviceInfo.MachineName = Environment.MachineName;
			serviceInfo.Version = "1.0"; //MessageServiceVersion.Current.Version;
			//serviceInfo.StartTime = _startTime.Value;
			//serviceInfo.ShutdownTime = _shutdownTime;
			//serviceInfo.ShutdownReason = this.shutdownReason;
			//serviceInfo.Running = _started;
			serviceInfo.ConfigFileName = _appConfig.ConfigFile;
			serviceInfo.BaseDir = AppDomain.CurrentDomain.BaseDirectory; //_busSettings.BaseDir;
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
			//serviceInfo.StartupError = _startupError;
		}
		#endregion

	}
}
