using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microservices.Bus.Configuration;
using Microservices.Configuration;

namespace Microservices.Bus
{
	public class AddinManager : IAddinManager
	{
		private readonly string _addinsDir;
		private readonly List<ChannelDescription> _registeredChannels;


		#region Ctor
		public AddinManager(BusSettings busSettings)
		{
			_addinsDir = busSettings.AddinsDir;
			_registeredChannels = new List<ChannelDescription>();
		}
		#endregion


		#region Properties
		public ChannelDescription[] RegisteredChannels
		{
			get => _registeredChannels.ToArray();
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
						lock (_registeredChannels)
						{
							_registeredChannels.Add(channelDescription);
						}
					}
					catch (Exception ex)
					{
						lock (errors)
						{
							errors.Add(ex);
						}
					}
				});

			if (errors.Count > 0)
				throw new AggregateException("Ошибка загрузки дополнений.", errors);
		}
		#endregion


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
