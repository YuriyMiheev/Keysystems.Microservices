using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Microservices.Configuration
{
	public static class XmlConfigFileExtensions
	{
		//public static IConfigurationBuilder AddXmlConfigFile(this IConfigurationBuilder builder, IFileProvider provider, string path, bool optional, bool reloadOnChange)
		//{
		//	return builder;
		//}

		//public static IConfigurationBuilder AddXmlConfigFile(this IConfigurationBuilder builder, Action<XmlConfigFileConfigurationSource> configureSource)
		//{
		//	return builder;
		//}

		public static IConfigurationBuilder AddXmlConfigFile(this IConfigurationBuilder builder, string path)
		{
			builder.Add(new XmlConfigFileConfigurationSource(path));
			return builder;
		}

		//public static IConfigurationBuilder AddXmlConfigFile(this IConfigurationBuilder builder, string path, bool optional)
		//{
		//	return builder;
		//}

		//public static IConfigurationBuilder AddXmlConfigFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
		//{
		//	return builder;
		//}

		//public static IConfigurationBuilder AddXmlConfigStream(this IConfigurationBuilder builder, Stream stream)
		//{
		//	return builder;
		//}

	}
}
