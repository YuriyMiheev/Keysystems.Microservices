using System;
using System.Collections.Generic;
using System.Linq;

using Microservices.Bus.Data;
using Microservices.Bus.Logging;

namespace Microservices.Bus.Channels
{
	public class ChannelManager : IChannelManager
	{
		private readonly IBusDataAdapter _dataAdapter;
		private readonly ILogger _logger;
		//private readonly ChannelFactory _factory;
		private readonly List<GroupInfo> _channelsGroups;
		private readonly List<IChannel> _channels;


		public ChannelManager(IBusDataAdapter dataAdapter, ILogger logger)
		{
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			//_factory = new ChannelFactory();
			_channelsGroups = new List<GroupInfo>();
			_channels = new List<IChannel>();
		}


		#region Properties
		/// <summary>
		/// {Get} Список групп каналов.
		/// </summary>
		public GroupInfo[] ChannelsGroups
		{
			get { return _channelsGroups.ToArray(); }
		}

		/// <summary>
		/// {Get} Список созданных каналов.
		/// </summary>
		public ChannelInfo[] RuntimeChannels
		{
			get { return _channels.Select(channel => channel.GetInfo()).OrderBy(channelInfo => channelInfo.LINK).ToArray(); }
		}
		#endregion


		public void LoadChannels()
		{
			_logger.LogTrace("Загрузка каналов.");
			var errors = new List<Exception>();

			GroupInfo deafaultGroup = GetOrCreateDefaultGroup();
			List<ChannelInfo> allChannels = _dataAdapter.GetChannels();
			List<GroupInfo> allGroups = _dataAdapter.GetChannelsGroups();

			allChannels.ForEach(channelInfo =>
				{
					try
					{
						// Если канал не принадлежит ни одной группе, то включаем его в группу по умолчанию
						if (!allGroups.Any(group => group.Channels.Contains(channelInfo.LINK)))
						{
							var map = new GroupChannelMap() { GroupLINK = deafaultGroup.LINK, ChannelLINK = channelInfo.LINK };
							_dataAdapter.SaveGroupsChannelsMap(map);
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
			RefreshGroups();

			// Создание и запуск каналов
			//channels.ForEach(channelInfo =>
			allChannels.AsParallel().ForAll(channelInfo =>
				{
					try
					{
						IChannel channel = CreateChannel(channelInfo);
						_dataAdapter.SaveChannelInfo(channelInfo);
						_channels.Add(channel);
					}
					catch (Exception ex)
					{
						lock (errors)
						{
							errors.Add(ex);
						}
					}
				});


			//this.channels.ToList().ForEach(channel =>
			_channels.AsParallel().ForAll(channel =>
				{
					if (channel.Enabled)
					{
						var settings = new ChannelSettings(channel.GetInfo().Properties);
						if (settings.AutoOpen)
						{
							try
							{
								channel.Open();
							}
							catch (Exception ex)
							{
								lock (errors)
								{
									errors.Add(ex);
								}
							}
						}
					}
				});

			//this.channels.ToList().ForEach(channel =>
			_channels.AsParallel().ForAll(channel =>
				{
					if (channel.IsOpened)
					{
						var settings = new ChannelSettings(channel.GetInfo().Properties);
						if (settings.AutoRun)
						{
							try
							{
								channel.Run();
							}
							catch (Exception ex)
							{
								lock (errors)
								{
									errors.Add(ex);
								}
							}
						}
					}
				});

			if (errors.Count > 0)
				throw new AggregateException(errors);
		}
	}
}
