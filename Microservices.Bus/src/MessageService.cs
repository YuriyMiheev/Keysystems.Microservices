using System;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Bus.Configuration;
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
			_database.Schema = _busSettings.Database.Schema;
			_database.ConnectionString = _busSettings.Database.ConnectionString;
			//_dataAdapter.ExecuteTimeout = (int)_busSettings.ExecuteTimeout.TotalSeconds;
			DbContext dbContext =_database.ValidateSchema();

			_addinManager.LoadAddins();
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
		#endregion


	}
}
