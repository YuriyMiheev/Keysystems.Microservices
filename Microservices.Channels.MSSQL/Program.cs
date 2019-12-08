using System.Linq;

using Microservices.Channels.Data;
using Microservices.Channels.Hubs;
using Microservices.Channels.Logging;
using Microservices.Channels.MSSQL.Data;
using Microservices.Configuration;
using Microservices.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microservices.Channels.MSSQL
{
	public class Program
	{
		private static IHost _host;

		public static void Main(string[] args)
		{
			IConfigurationRoot hostConfiguration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", true, false)
				.AddCommandLine(args)
				.Build();
			IConfigurationRoot appConfiguration = new ConfigurationBuilder()
				.AddXmlConfigFile("appsettings.config")
				.Build();

			IHostBuilder hostBuilder = Host.CreateDefaultBuilder()
				.ConfigureHostConfiguration(configBuilder => configBuilder.AddConfiguration(hostConfiguration))
				.ConfigureWebHostDefaults(webBuilder =>
					{
						// Несколько вызовов ConfigureServices добавляются друг к другу.
						//При наличии нескольких вызовов метода Configure используется последний вызов Configure.
						webBuilder
							.UseConfiguration(hostConfiguration)
							.UseStartup<Startup>();
					})
				.ConfigureServices(services =>
					{
						var appConfig = (XmlConfigFileConfigurationProvider)appConfiguration.Providers.Single();
						services.AddSingleton<IAppSettingsConfig>(appConfig);

						//IDatabase database = new ChannelDatabase();
						//database.Schema = _databaseSettings.Schema;
						//database.ConnectionString = _infoSettings.RealAddress;
						//DbContext dbContext = database.CreateOrUpdateSchema();
						//DbContext dbContext = database.ValidateSchema();

						services.AddSingleton<IDatabase, ChannelDatabase>();
						services.AddSingleton<IChannelMessageDataAdapter, MessageDataAdapter>();
						services.AddSingleton<ILogger, ChannelLogger>();
						services.AddSingleton<IHubConnections, HubConnections>();
						services.AddSingleton<ISendMessageScanner, SendMessageScanner>();
						services.AddSingleton<IMessageReceiver, MessageReceiver>();
						services.AddSingleton<ChannelStatus>();
						services.AddSingleton<IChannelService>(serviceProvider =>
							{
								return new ChannelService(serviceProvider);
							});
						services.AddHostedService<IChannelService>(serviceProvider =>
							{
								return serviceProvider.GetRequiredService<IChannelService>();
							});
					});

			_host = hostBuilder.Build();
			_host.Run();
		}
	}
}
