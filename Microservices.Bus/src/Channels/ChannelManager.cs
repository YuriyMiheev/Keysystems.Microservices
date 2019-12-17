using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

using Microservices.Bus.Addins;
using Microservices.Bus.Configuration;
using Microservices.Bus.Data;
using Microservices.Bus.Logging;

namespace Microservices.Bus.Channels
{
	public class ChannelManager : IChannelManager
	{
		private readonly IBusDataAdapter _dataAdapter;
		private readonly ILogger _logger;
		private readonly IChannelFactory _channelFactory;
		private readonly ConcurrentDictionary<string, GroupInfo> _channelsGroups;
		private readonly ConcurrentDictionary<ChannelInfo, IChannel> _channels;
		private readonly IAddinManager _addinManager;
		private readonly BusSettings _busSettings;


		public ChannelManager(IAddinManager addinManager, IChannelFactory channelFactory, BusSettings busSettings, IBusDataAdapter dataAdapter, ILogger logger)
		{
			_addinManager = addinManager ?? throw new ArgumentNullException(nameof(addinManager));
			_channelFactory = channelFactory ?? throw new ArgumentNullException(nameof(channelFactory));
			_busSettings = busSettings ?? throw new ArgumentNullException(nameof(addinManager));
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_channelsGroups = new ConcurrentDictionary<string, GroupInfo>();
			_channels = new ConcurrentDictionary<ChannelInfo, IChannel>();
		}


		#region Properties
		/// <summary>
		/// {Get} Список групп каналов.
		/// </summary>
		public GroupInfo[] ChannelsGroups
		{
			get { return _channelsGroups.Values.ToArray(); }
		}

		/// <summary>
		/// {Get} Список созданных каналов.
		/// </summary>
		public ChannelInfo[] RuntimeChannels
		{
			get { return _channels.Keys.OrderBy(channelInfo => channelInfo.LINK).ToArray(); }
		}
		#endregion


		#region Channels
		public void LoadChannels()
		{
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
			//allChannels.ForEach(channelInfo =>
			allChannels.AsParallel().ForAll(channelInfo =>
				{
					try
					{
						ChannelDescription description = _addinManager.FindChannelDescription(channelInfo.Provider);
						if (description == null)
							channelInfo.Enabled = false;
						else
							channelInfo.SetDescription(description);

						if (channelInfo.IsSystem)
							channelInfo.RealAddress = _busSettings.Database.ConnectionString;

						_dataAdapter.SaveChannel(channelInfo);

						if (channelInfo.Enabled)
						{
							IChannel channel = _channelFactory.CreateChannel(channelInfo);
							if (!_channels.TryAdd(channelInfo, channel))
							{
								channel.Dispose();
								throw new InvalidOperationException($"Канал {channelInfo.VirtAddress} уже существует.");
							}
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


			//_channels.ToList().ForEach(item =>
			_channels.AsParallel().ForAll(async item =>
				{
					ChannelInfo channelInfo = item.Key;
					IChannel channel = item.Value;

					if (channel != null)
					{
						ChannelSettings settings = channelInfo.ChannelSettings();
						if (settings.AutoOpen)
						{
							try
							{
								await channel.OpenAsync();
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

			//_channels.ToList().ForEach(item =>
			_channels.AsParallel().ForAll(async item =>
				{
					ChannelInfo channelInfo = item.Key;
					IChannel channel = item.Value;

					if (channel != null && channel.IsOpened)
					{
						ChannelSettings settings = channelInfo.ChannelSettings();
						if (settings.AutoRun)
						{
							try
							{
								await channel.RunAsync();
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

		//private IChannel CreateChannel(ChannelInfo channelInfo)
		//{
		//	ChannelDescription description = _addinManager.FindChannelDescription(channelInfo.Provider);
		//	if (description == null)
		//		channelInfo.Enabled = false;
		//	else
		//		channelInfo.SetDescription(description);

		//	if (channelInfo.IsSystem)
		//		channelInfo.RealAddress = _busSettings.Database.ConnectionString;

		//	return _channelFactory.CreateChannel(channelInfo);
		//}

		public void OpenChannel(ChannelInfo channelInfo)
		{
		}
		#endregion


		#region Groups
		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupLink"></param>
		/// <returns></returns>
		public GroupInfo FindGroupInfo(int groupLink)
		{
			return _channelsGroups.Values.SingleOrDefault(group => group.LINK == groupLink);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupLink"></param>
		/// <returns></returns>
		public GroupInfo GetGroupInfo(int groupLink)
		{
			GroupInfo groupInfo = FindGroupInfo(groupLink);
			if (groupInfo == null)
				throw new InvalidOperationException($"Группа каналов #{groupLink} не найдена или не существует.");

			return groupInfo;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		public GroupInfo FindGroupInfo(string groupName)
		{
			return _channelsGroups.Values.SingleOrDefault(group => group.Name == groupName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		public GroupInfo GetGroupInfo(string groupName)
		{
			GroupInfo groupInfo = FindGroupInfo(groupName);
			if (groupInfo == null)
				throw new InvalidOperationException($"Группа каналов \"{groupName}\" не найдена или не существует.");

			return groupInfo;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupParams"></param>
		/// <returns></returns>
		public GroupInfo CreateGroup(GroupCreationParams groupParams)
		{
			#region Validate parameters
			if (groupParams == null)
				throw new ArgumentNullException(nameof(groupParams));

			if (String.IsNullOrWhiteSpace(groupParams.Name))
				throw new ArgumentException("Не задано имя группы каналов.", nameof(groupParams.Name));
			#endregion

			var groupInfo = new GroupInfo(groupParams.Name);
			if (!String.IsNullOrWhiteSpace(groupParams.Image))
				groupInfo.Image = groupParams.Image;

			_dataAdapter.SaveChannelsGroup(groupInfo);
			if (!_channelsGroups.TryAdd(groupInfo.Name, groupInfo))
				throw new InvalidOperationException($"Группа каналов с именем {groupInfo.Name} уже существует.");

			return groupInfo;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupLink"></param>
		public void DeleteGroup(int groupLink)
		{
			GroupInfo groupInfo = FindGroupInfo(groupLink);
			if (groupInfo == null)
				return;

			_dataAdapter.DeleteChannelsGroup(groupInfo);
			_channelsGroups.TryRemove(groupInfo.Name, out GroupInfo g);

			GroupInfo defaultGroup = GetOrCreateDefaultGroup();
			List<GroupChannelMap> maps = _dataAdapter.GetGroupsChannelsMap();

			foreach (int channelLink in groupInfo.Channels)
			{
				GroupChannelMap map = maps.SingleOrDefault(x => x.GroupLINK == groupLink && x.ChannelLINK == channelLink);
				if (map != null)
					_dataAdapter.DeleteGroupsChannelsMap(map);

				GroupInfo[] groups = GetChannelGroups(channelLink);
				if (groups.Count() == 0)
				{
					map = new GroupChannelMap() { GroupLINK = defaultGroup.LINK, ChannelLINK = channelLink };
					_dataAdapter.SaveGroupsChannelsMap(map);
				}
			}

			RefreshGroups();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="updateParams"></param>
		public void UpdateGroupInfo(GroupInfoUpdateParams updateParams)
		{
			GroupInfo groupInfo = GetGroupInfo(updateParams.GroupLink);
			groupInfo.Name = updateParams.Name;
			groupInfo.Image = updateParams.Image;

			_dataAdapter.SaveChannelsGroup(groupInfo);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channelLink"></param>
		/// <returns></returns>
		public GroupInfo[] GetChannelGroups(int channelLink)
		{
			return _channelsGroups.Values.Where(group => group.Channels.Contains(channelLink)).ToArray();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupLink"></param>
		/// <param name="channelLink"></param>
		public void AddChannelToGroup(int groupLink, int channelLink)
		{
			List<GroupChannelMap> maps = _dataAdapter.GetGroupsChannelsMap();
			GroupChannelMap map = maps.SingleOrDefault(x => x.GroupLINK == groupLink && x.ChannelLINK == channelLink);
			if (map == null)
			{
				map = new GroupChannelMap()
				{
					GroupLINK = groupLink,
					ChannelLINK = channelLink
				};

				_dataAdapter.SaveGroupsChannelsMap(map);

				RefreshGroups();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupLink"></param>
		/// <param name="channelLink"></param>
		public void RemoveChannelFromGroup(int groupLink, int channelLink)
		{
			RemoveFromGroupInternal(groupLink, channelLink, false);
		}

		private void RemoveFromGroupInternal(int groupLink, int channelLink, bool force)
		{
			List<GroupChannelMap> maps = _dataAdapter.GetGroupsChannelsMap();
			GroupChannelMap map = maps.SingleOrDefault(x => x.GroupLINK == groupLink && x.ChannelLINK == channelLink);
			if (map == null)
				return;

			GroupInfo defaultGroup = GetOrCreateDefaultGroup();

			GroupInfo[] groups = GetChannelGroups(channelLink);
			if (groups.Count() == 1)
			{
				if (force)
				{
					_dataAdapter.DeleteGroupsChannelsMap(map);
				}
				else
				{
					if (groups[0].LINK != defaultGroup.LINK)
					{
						map.GroupLINK = defaultGroup.LINK;
						_dataAdapter.SaveGroupsChannelsMap(map);
					}
				}
			}
			else if (groups.Count() > 1)
			{
				_dataAdapter.DeleteGroupsChannelsMap(map);
			}

			RefreshGroups();
		}

		public GroupInfo GetOrCreateDefaultGroup()
		{
			string defaultGroupName = GroupInfo.DefaultGroupName;
			List<GroupInfo> groups = _channelsGroups.Values.ToList();

			GroupInfo deafaultGroup = groups.SingleOrDefault(group => group.Name == defaultGroupName);
			if (deafaultGroup == null)
			{
				groups = _dataAdapter.GetChannelsGroups();
				deafaultGroup = groups.SingleOrDefault(group => group.Name == defaultGroupName);
				if (deafaultGroup == null)
				{
					deafaultGroup = new GroupInfo(defaultGroupName);
					_dataAdapter.SaveChannelsGroup(deafaultGroup);
				}

				_channelsGroups.TryAdd(deafaultGroup.Name, deafaultGroup);
			}

			return deafaultGroup;
		}

		private void RefreshGroups()
		{
			List<GroupInfo> groups = _dataAdapter.GetChannelsGroups();
			_channelsGroups.Clear();
			foreach (GroupInfo group in groups)
			{
				_channelsGroups.TryAdd(group.Name, group);
			}
		}
		#endregion

	}
}
