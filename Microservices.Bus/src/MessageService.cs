using System;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Bus.Configuration;
using Microservices.Bus.Data;
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
		private readonly IServiceInfoManager _serviceInfoManager;


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
			_serviceInfoManager = serviceProvider.GetRequiredService<IServiceInfoManager>();
		}
		#endregion


		#region IHostedService
		public Task StartAsync(CancellationToken cancellationToken)
		{
			return Task.Run(() =>
				{
					try
					{
						_database.Schema = _busSettings.Database.Schema;
						_database.ConnectionString = _busSettings.Database.ConnectionString;
						//_dataAdapter.ExecuteTimeout = (int)_databaseSettings.ExecuteTimeout.TotalSeconds;
						//using DbContext dbContext = _database.ValidateSchema();

						_addinManager.LoadAddins();

						//_licManager.LoadLicenses();
						//_channelManager.LoadChannels();

						//SetCurrentParamsTo(_serviceInfo);
						//_serviceInfoManager.SaveInfo(false);
					}
					catch (Exception ex)
					{
						//_logger.LogError(ex);
						//this.startupError = new InvalidOperationException("Ошибка запуска сервиса.", ex);
						//throw this.startupError;
						throw;
					}
				}, cancellationToken);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
		#endregion


		#region Helpers
		private void SetCurrentParamsTo(ServiceInfo serviceInfo)
		{
			//serviceInfo.InstanceID = this.instanceId;
			//serviceInfo.ServiceName = SERVICE_NAME;
			serviceInfo.MachineName = Environment.MachineName;
			//serviceInfo.Version = MessageServiceVersion.Current.Version;
			//serviceInfo.StartTime = (this.startTime ?? DateTime.Now);
			//serviceInfo.ShutdownTime = this.shutdownTime;
			//serviceInfo.ShutdownReason = this.shutdownReason;
			//serviceInfo.Running = this.started;
			//serviceInfo.ConfigFileName = _busSettings.ConfigFileName;
			//serviceInfo.BaseDir = _busSettings.BaseDir;
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
			//serviceInfo.StartupError = (this.startupError != null ? this.startupError.Wrap() : null);
		}
		#endregion

	}
}
