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
				//.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", true, false)
				.AddCommandLine(args)
				.Build();

			IConfigurationRoot channelConfig = new ConfigurationBuilder()
				//.SetBasePath(Directory.GetCurrentDirectory())
				.AddXmlConfigFile("channel.config")
				.Build();
			var channelConfigProvider = channelConfig.Providers.Cast<XmlConfigFileConfigurationProvider>().Single();

			IConfigurationRoot serviceConfig = new ConfigurationBuilder()
				//.SetBasePath(Directory.GetCurrentDirectory())
				.AddXmlConfigFile("service.config")
				.Build();
			var serviceConfigProvider = serviceConfig.Providers.Cast<XmlConfigFileConfigurationProvider>().Single();

			//IHostBuilder hostBuilder = new HostBuilder()
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
						var service = new ChannelService(null, new ChannelConfigFileSettings(channelConfigProvider), new ServiceConfigFileSettings(serviceConfigProvider));
						services.AddSingleton<IChannelService>(service);
					});

			_host = hostBuilder.Build();
			_host.Run();
		}
	}
}
