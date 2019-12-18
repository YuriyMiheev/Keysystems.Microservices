using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

using Microservices.Bus.Channels;
using Microservices.Bus.Configuration;
using Microservices.Configuration;

namespace Microservices.Bus.Addins
{
	public class AddinManager : IAddinManager
	{
		private readonly string _addinsDir;
		private readonly ConcurrentDictionary<string, ChannelDescription> _registeredChannels;


		#region Ctor
		public AddinManager(BusSettings busSettings)
		{
			_addinsDir = busSettings.AddinsDir;
			_registeredChannels = new ConcurrentDictionary<string, ChannelDescription>();
		}
		#endregion


		#region Properties
		public ChannelDescription[] RegisteredChannels
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
						ChannelDescription channelDescription = LoadAddin(dir);
						if (!_registeredChannels.TryAdd(channelDescription.Provider, channelDescription))
							throw new InvalidOperationException($"Канал типа {channelDescription.Provider} уже существует.");
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
		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public ChannelDescription FindChannelDescription(string provider)
		{
			#region Validate parameters
			if (String.IsNullOrWhiteSpace(provider))
				throw new ArgumentException("provider");
			#endregion

			if (_registeredChannels.ContainsKey(provider))
				return _registeredChannels[provider];
			else
				return null;

			//if (description == null)
			//	return null;

			//return new ChannelDescription(description.GetMetadata()) { BinPath = description.BinPath };
		}


		#region Helpers
		private ChannelDescription LoadAddin(string dir)
		{
			string configFile = Path.Combine(dir, "appsettings.config");
			using var appConfguration = new XmlConfigFileConfigurationProvider(configFile);
			appConfguration.Load();

			var channelDescription = new ChannelDescription(appConfguration.GetAppSettings());
			channelDescription.BinPath = Path.Combine(dir, channelDescription.Type);
			return channelDescription;
		}
		#endregion

	}
}
