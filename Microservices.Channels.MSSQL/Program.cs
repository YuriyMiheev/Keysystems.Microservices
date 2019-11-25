using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Xml;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Microservices.Channels.Configuration;
//using Microservices.Channels.MSSQL.Configuration;

namespace Microservices.Channels.MSSQL
{
	public class Program
	{
		private static IHost _host;

		public static void Main(string[] args)
		{
			IConfigurationRoot appConfig = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", true, false)
				.AddCommandLine(args)
				.Build();
			IConfigurationRoot channelConfig = new ConfigurationBuilder()
				.AddXmlConfigFile("channel.config")
				.Build();
			IConfigurationRoot serviceConfig = new ConfigurationBuilder()
				.AddXmlConfigFile("service.config")
				.Build();

			IHostBuilder hostBuilder = Host.CreateDefaultBuilder()
				.ConfigureHostConfiguration(configBuilder => configBuilder.AddConfiguration(appConfig))
				.ConfigureWebHostDefaults(webBuilder =>
					{
						// Несколько вызовов ConfigureServices добавляются друг к другу.
						//При наличии нескольких вызовов метода Configure используется последний вызов Configure.
						webBuilder.UseConfiguration(appConfig)
							.UseStartup<Startup>();
					})
				.ConfigureServices(services =>
					{
						services.AddSingleton<IChannelService>(serviceProvider =>
							{
								var channelConfigProvider = channelConfig.Providers.Single() as XmlConfigFileConfigurationProvider;
								var serviceConfigProvider = serviceConfig.Providers.Single() as XmlConfigFileConfigurationProvider;
								return new ChannelService(serviceProvider, new ChannelConfigFileSettings(channelConfigProvider), new ServiceConfigFileSettings(serviceConfigProvider));
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
