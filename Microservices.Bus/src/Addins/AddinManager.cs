using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microservices.Bus.Channels;
using Microservices.Bus.Configuration;
using Microservices.Channels.Configuration;
using Microservices.Configuration;

namespace Microservices.Bus.Addins
{
	public class AddinManager : IAddinManager
	{
		private readonly string _addinsDir;
		private readonly ConcurrentDictionary<string, MicroserviceDescription> _registeredChannels;


		#region Ctor
		public AddinManager(BusSettings busSettings)
		{
			_addinsDir = busSettings.AddinsDir;
			_registeredChannels = new ConcurrentDictionary<string, MicroserviceDescription>();
		}
		#endregion


		#region Properties
		public MicroserviceDescription[] RegisteredMicroservices
		{
			get => _registeredChannels.Values.ToArray();
		}
		#endregion


		#region Methods
		public void LoadAddins()
		{
			var errors = new List<Exception>();

			List<string> addinDirs = Directory.GetDirectories(_addinsDir, "*.Addin", SearchOption.TopDirectoryOnly).ToList();
			//addinDirs.ForEach(dir =>
			addinDirs.AsParallel().ForAll(dir =>
				{
					try
					{
						MicroserviceDescription description = LoadAddin(dir);
						if (!_registeredChannels.TryAdd(description.Provider, description))
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

			if (_registeredChannels.ContainsKey(provider))
				return _registeredChannels[provider];
			else
				return null;
		}
		#endregion


		#region Helpers
		private MicroserviceDescription LoadAddin(string dir)
		{
			string configFile = Path.Combine(dir, "appsettings.config");
			using var appConfguration = new XmlConfigFileConfigurationProvider(configFile);
			appConfguration.Load();

			var description = new MicroserviceDescription(appConfguration.GetAppSettings());
			description.BinPath = dir;
			return description;
		}
		#endregion

	}
}
