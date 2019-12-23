using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microservices.Bus.Channels;
using Microservices.Bus.Configuration;
using Microservices.Configuration;

namespace Microservices.Bus.Addins
{
	public class AddinManager : IAddinManager
	{
		private readonly AddinManagerOptions _options;
		private readonly ConcurrentDictionary<string, MicroserviceDescription> _registeredMicroservices;


		#region Ctor
		public AddinManager(AddinManagerOptions options)
		{
			_options = options ?? throw new ArgumentNullException(nameof(options));
			_registeredMicroservices = new ConcurrentDictionary<string, MicroserviceDescription>();
		}
		#endregion


		#region Properties
		public MicroserviceDescription[] RegisteredMicroservices
		{
			get => _registeredMicroservices.Values.ToArray();
		}
		#endregion


		#region Methods
		public void LoadAddins()
		{
			var errors = new List<Exception>();

			List<string> addinDirs = Directory.GetDirectories(_options.AddinsDirectory, "*.Addin", SearchOption.TopDirectoryOnly).ToList();
			addinDirs.ForEach(dir =>
			//addinDirs.AsParallel().ForAll(dir =>
				{
					try
					{
						MicroserviceDescription description = LoadAddin(dir);
						if (!_registeredMicroservices.TryAdd(description.Provider, description))
							throw new InvalidOperationException($"Канал типа {description.Provider} уже существует.");
					}
					catch (Exception ex)
					{
						lock (errors)
						{
							var error = new InvalidOperationException($"Ошибка загрузки дополнения из \"{dir}\".", ex);
							errors.Add(error);
						}
					}
				});

			if (errors.Count > 0)
				throw new AggregateException(errors);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public MicroserviceDescription FindMicroservice(string provider)
		{
			#region Validate parameters
			if (String.IsNullOrWhiteSpace(provider))
				throw new ArgumentException(nameof(provider));
			#endregion

			if (_registeredMicroservices.ContainsKey(provider))
				return _registeredMicroservices[provider];
			else
				return null;
		}
		#endregion


		#region Helpers
		private MicroserviceDescription LoadAddin(string dir)
		{
			string configFilePath = Path.Combine(dir, _options.ConfigFileName);
			using var appConfguration = new XmlConfigFileConfigurationProvider(configFilePath);
			appConfguration.Load();

			var description = new MicroserviceDescription(appConfguration.GetAppSettings());
			description.BinPath = dir;
			return description;
		}
		#endregion

	}
}
