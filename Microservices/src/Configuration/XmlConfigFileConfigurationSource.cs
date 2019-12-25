using System;

using Microsoft.Extensions.Configuration;

namespace Microservices.Configuration
{
	public class XmlConfigFileConfigurationSource : FileConfigurationSource
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		public XmlConfigFileConfigurationSource(string path)
		{
			this.Path = path;
			this.OnLoadException = new Action<FileLoadExceptionContext>(context =>
			{
				context.Ignore = false;
			});
		}


		public override IConfigurationProvider Build(IConfigurationBuilder builder)
		{
			return new XmlConfigFileConfigurationProvider(this.Path);
		}
	}
}
