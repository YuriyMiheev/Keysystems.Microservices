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
						webBuilder.UseConfiguration(hostConfiguration)
							.UseStartup<Startup>();
					})
				.ConfigureServices(services =>
					{
						services.AddSingleton<IChannelService>(serviceProvider =>
							{
								return new ChannelService(serviceProvider, appConfiguration);
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
