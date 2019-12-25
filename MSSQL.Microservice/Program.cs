using System.Linq;

using Microservices.Channels;
using Microservices.Channels.Data;
using Microservices.Channels.Hubs;
using Microservices.Channels.Logging;
using Microservices.Configuration;
using Microservices.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MSSQL.Microservice.Data;

namespace MSSQL.Microservice
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
						services.AddSingleton<IDatabase, ChannelDatabase>();
						services.AddSingleton<IChannelDataAdapter, ChannelDataAdapter>();
						services.AddSingleton<ILogger, ChannelLogger>();
						services.AddSingleton<IHubConnections, HubConnections>();
						services.AddSingleton<IMessageScanner, MessageScanner>();
						services.AddSingleton<IMessageReceiver, MessageReceiver>();
						services.AddSingleton<ChannelStatus>();
						services.AddSingleton<IChannelService, ChannelService>();
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
