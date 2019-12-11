using System.Linq;

using Microservices.Bus.Addins;
using Microservices.Bus.Channels;
using Microservices.Bus.Configuration;
using Microservices.Bus.Data;
using Microservices.Bus.Data.MSSQL;
using Microservices.Bus.Licensing;
using Microservices.Bus.Logging;
using Microservices.Configuration;
using Microservices.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microservices.Bus
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
						webBuilder
							.UseConfiguration(hostConfiguration)
							.UseStartup<Startup>();
					})
				.ConfigureServices(services =>
					{
						var appConfig = (XmlConfigFileConfigurationProvider)appConfiguration.Providers.Single();
						services.AddSingleton<IAppSettingsConfig>(appConfig);
						services.AddSingleton<IConnectionStringsConfig>(appConfig);
						services.AddSingleton<BusSettings>();
						services.AddSingleton<IDatabase, SysDatabase>();
						services.AddSingleton<IBusDataAdapter, BusDataAdapter>();
						services.AddSingleton<ILogger, BusLogger>();
						services.AddSingleton<IAuthManager, AuthManager>();
						services.AddSingleton<IChannelManager, ChannelManager>();
						services.AddSingleton<IAddinManager, AddinManager>();
						services.AddSingleton<ILicenseManager, LicenseManager>();
						services.AddSingleton<ServiceInfo>();
						services.AddSingleton<IServiceInfoManager, ServiceInfoManager>();
						services.AddSingleton<IMessageService>(serviceProvider =>
							{
								return new MessageService(serviceProvider);
							});
						services.AddHostedService<IMessageService>(serviceProvider =>
							{
								return serviceProvider.GetRequiredService<IMessageService>();
							});
					});

			_host = hostBuilder.Build();
			_host.Run();
		}
	}
}
