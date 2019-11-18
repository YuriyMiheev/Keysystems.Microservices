using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Microservices.Channels.Configuration;

namespace Microservices.Channels.MSSQL.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class ServiceConfig : IServiceConfig
	{
		private IServiceConfigFileSettings _settings;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="settings"></param>
		public ServiceConfig(IServiceConfigFileSettings settings)
		{
			_settings = settings ?? throw new ArgumentNullException("settings");
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public bool IsConfigured { get; private set; }

		//private IServiceCache cache;
		///// <summary>
		///// {Get}
		///// </summary>
		//public IServiceCache Cache
		//{
		//	get
		//	{
		//		if (cache == null)
		//			throw new ConfigException("Кэш не определен.");

		//		return cache;
		//	}
		//	private set { cache = value; }
		//}

		//private ILogger logger;
		///// <summary>
		///// {Get}
		///// </summary>
		//public ILogger Logger
		//{
		//	get
		//	{
		//		if (logger == null)
		//			throw new ConfigException("Логер не определен.");

		//		return logger;
		//	}
		//	private set { logger = value; }
		//}

		/// <summary>
		/// {Get}
		/// </summary>
		public string TempDir
		{
			get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "TEMP"); }
		}
		#endregion


		#region Methods
		public IServiceConfig CreateWorkingDirectories()
		{
			//Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "DRAFTS"));
			//Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "GUIDS"));
			//Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "LOGS"));
			//Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "TEMP"));
			//Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "UPLOADS"));
			//Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "TOOLS"));
			return this;
		}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="cache"></param>
		///// <returns></returns>
		//public IServiceConfig DefineCache(IServiceCache cache)
		//{
		//	CheckConfigured();

		//	#region Validate parameters
		//	if (cache == null)
		//		throw new ArgumentNullException("cache");
		//	#endregion

		//	this.Cache = cache;
		//	return this;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="logger"></param>
		///// <returns></returns>
		//public IServiceConfig DefineLogger(ILogger logger)
		//{
		//	CheckConfigured();

		//	#region Validate parameters
		//	if (logger == null)
		//		throw new ArgumentNullException("logger");
		//	#endregion

		//	this.Logger = logger;
		//	return this;
		//}

		/// <summary>
		/// 
		/// </summary>
		public void EndConfigure()
		{
			this.IsConfigured = true;
		}
		#endregion


		#region Helpers
		private void CheckConfigured()
		{
			if (this.IsConfigured)
				throw new InvalidOperationException("Сервис уже сконфигурирован.");
		}
		#endregion

	}
}
