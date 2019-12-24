using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microservices.Configuration;

namespace Microservices.Bus.Addins
{
	public class AddinManager : IAddinManager
	{
		private readonly AddinManagerOptions _options;
		private readonly ConcurrentDictionary<string, IAddinDescription> _registeredChannels;


		#region Ctor
		public AddinManager(AddinManagerOptions options)
		{
			_options = options ?? throw new ArgumentNullException(nameof(options));
			_registeredChannels = new ConcurrentDictionary<string, IAddinDescription>();
		}
		#endregion


		#region Properties
		public IAddinDescription[] RegisteredChannels
		{
			get => _registeredChannels.Values.ToArray();
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
						IAddinDescription description = LoadAddin(dir);
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
		public IAddinDescription FindDescription(string provider)
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
		private IAddinDescription LoadAddin(string dir)
		{
			string configFilePath = Path.Combine(dir, _options.ConfigFileName);
			using var appConfguration = new XmlConfigFileConfigurationProvider(configFilePath);
			appConfguration.Load();

			var description = new AddinDescription(appConfguration.GetAppSettings());
			description.BinPath = dir;
			return description;
		}
		#endregion

	}
}
