using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microservices.Channels.Configuration;

namespace Microservices.Channels.MSSQL.Configuration
{
	public static class ChannelServiceDependencyInjectionExtensions
	{
		public static IServiceCollection AddChannelService(this IServiceCollection services)
		{
			IConfigurationRoot channelConfig = new ConfigurationBuilder()
				.AddXmlConfigFile("channel.config")
				.Build();
			var channelConfigProvider = channelConfig.Providers.Cast<XmlConfigFileConfigurationProvider>().Single();

			IConfigurationRoot serviceConfig = new ConfigurationBuilder()
				.AddXmlConfigFile("service.config")
				.Build();
			var serviceConfigProvider = serviceConfig.Providers.Cast<XmlConfigFileConfigurationProvider>().Single();

			services.AddSingleton<IChannelService>(serviceProvider =>
			{
				var channelService = new ChannelService(serviceProvider, new ChannelConfigFileSettings(channelConfigProvider), new ServiceConfigFileSettings(serviceConfigProvider));
				return channelService;
			});

			return services;
		}
	}
}
