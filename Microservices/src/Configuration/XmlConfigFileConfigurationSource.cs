using System;
using System.Collections.Generic;
using System.Text;

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
		}


		public override IConfigurationProvider Build(IConfigurationBuilder builder)
		{
			return new XmlConfigFileConfigurationProvider(this.Path);
		}
	}
}
