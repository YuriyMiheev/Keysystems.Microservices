using Microservices.Configuration;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
				.AddXmlConfigFile("Rms.config")
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
					});

			_host = hostBuilder.Build();
			_host.Run();
		}
	}
}
