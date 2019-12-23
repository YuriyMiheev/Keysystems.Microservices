using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

using Microservices.Bus.Addins;
using Microservices.Bus.Configuration;
using Microservices.Bus.Data;
using Microservices.Bus.Logging;
using Microservices.Channels.Configuration;
using Microservices.Channels;

namespace Microservices.Bus.Channels
{
	public class ChannelManager : IChannelManager
	{
		private readonly IBusDataAdapter _dataAdapter;
		private readonly ILogger _logger;
		private readonly IChannelContextFactory _contextFactory;
		private readonly ConcurrentDictionary<string, GroupInfo> _groups;
		private readonly ConcurrentDictionary<string, IChannelContext> _channels;
		private readonly IAddinManager _addinManager;
		private readonly BusSettings _busSettings;


		public ChannelManager(IAddinManager addinManager, IChannelContextFactory contextFactory, BusSettings busSettings, IBusDataAdapter dataAdapter, ILogger logger)
		{
			_addinManager = addinManager ?? throw new ArgumentNullException(nameof(addinManager));
			_contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
			_busSettings = busSettings ?? throw new ArgumentNullException(nameof(addinManager));
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_groups = new ConcurrentDictionary<string, GroupInfo>();
			_channels = new ConcurrentDictionary<string, IChannelContext>();
		}


		#region Properties
		/// <summary>
		/// {Get} Список групп каналов.
		/// </summary>
		public GroupInfo[] ChannelsGroups
		{
			get { return _groups.Values.OrderBy(group => group.LINK).ToArray(); }
		}

		/// <summary>
		/// {Get} Список созданных каналов.
		/// </summary>
		public IChannelContext[] RuntimeChannels
		{
			get { return _channels.Values.OrderBy(context => context.Info.LINK).ToArray(); }
		}
		#endregion


		#region Channels
		public void LoadChannels()
		{
			var errors = new List<Exception>();

			GroupInfo deafaultGroup = GetOrCreateDefaultGroup();
			List<ChannelInfo> allChannels = _dataAdapter.GetChannels();
			List<GroupInfo> allGroups = _dataAdapter.GetGroups();

			allChannels.ForEach(channelInfo =>
				{
					try
					{
						// Если канал не принадлежит ни одной группе, то включаем его в группу по умолчанию
						if (!allGroups.Any(group => group.Channels.Contains(channelInfo.LINK)))
						{
							var map = new GroupChannelMap() { GroupLINK = deafaultGroup.LINK, ChannelLINK = channelInfo.LINK };
							_dataAdapter.SaveGroupMap(map);
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
			allChannels.ForEach(channelInfo =>
			//allChannels.AsParallel().ForAll(channelInfo =>
			{
				try
				{
					MicroserviceDescription description = _addinManager.FindMicroservice(channelInfo.Provider);
					if (description == null)
						channelInfo.Enabled = false;
					else
						channelInfo.SetDescription(description);

					if (channelInfo.IsSystem())
						channelInfo.RealAddress = _busSettings.Database.ConnectionString;

					_dataAdapter.SaveChannelInfo(channelInfo);

					if (channelInfo.Enabled)
					{
						IChannelContext context = _contextFactory.CreateContext(channelInfo);
						if (!_channels.TryAdd(channelInfo.VirtAddress, context))
						{
							context.Dispose();
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


			_channels.Values.ToList().ForEach(context =>
			//_channels.Values.AsParallel().ForAll(context =>
			{
				ChannelSettings channelSettings = context.Info.ChannelSettings();
				if (channelSettings.AutoOpen)
				{
					try
					{
						IChannel channel = context.CreateChannelAsync().Result;
						IMicroserviceClient client = context.Client;

						var microserviceSettings = new Dictionary<string, string>
						{
							{ ".RealAddress", "" }
						};
						client.SetSettingsAsync(microserviceSettings).Wait();

						channel.OpenAsync().Wait();
					}
					catch (Exception ex)
					{
						lock (errors)
						{
							errors.Add(ex);
						}
					}
				}
			});

			_channels.Values.ToList().ForEach(context =>
			//_channels.Values.AsParallel().ForAll(context =>
			{
				ChannelInfo channelInfo = context.Info;
				ChannelStatus channelStatus = context.Status;
				IChannel channel = context.Channel;
				if (channel != null && channelStatus.Opened)
				{
					ChannelSettings settings = channelInfo.ChannelSettings();
					if (settings.AutoRun)
					{
						try
						{
							channel.RunAsync().Wait();
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

		public void TerminateChannel(string virtAddress)
		{
			//_channels[
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
			return _groups.Values.SingleOrDefault(group => group.LINK == groupLink);
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
			return _groups.Values.SingleOrDefault(group => group.Name == groupName);
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

			_dataAdapter.SaveGroup(groupInfo);
			if (!_groups.TryAdd(groupInfo.Name, groupInfo))
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

			_dataAdapter.DeleteGroup(groupInfo);
			_groups.TryRemove(groupInfo.Name, out GroupInfo g);

			GroupInfo defaultGroup = GetOrCreateDefaultGroup();
			List<GroupChannelMap> maps = _dataAdapter.GetGroupMap();

			foreach (int channelLink in groupInfo.Channels)
			{
				GroupChannelMap map = maps.SingleOrDefault(x => x.GroupLINK == groupLink && x.ChannelLINK == channelLink);
				if (map != null)
					_dataAdapter.DeleteGroupMap(map);

				GroupInfo[] groups = GetChannelGroups(channelLink);
				if (groups.Count() == 0)
				{
					map = new GroupChannelMap() { GroupLINK = defaultGroup.LINK, ChannelLINK = channelLink };
					_dataAdapter.SaveGroupMap(map);
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

			_dataAdapter.SaveGroup(groupInfo);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channelLink"></param>
		/// <returns></returns>
		public GroupInfo[] GetChannelGroups(int channelLink)
		{
			return _groups.Values.Where(group => group.Channels.Contains(channelLink)).ToArray();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupLink"></param>
		/// <param name="channelLink"></param>
		public void AddChannelToGroup(int groupLink, int channelLink)
		{
			List<GroupChannelMap> maps = _dataAdapter.GetGroupMap();
			GroupChannelMap map = maps.SingleOrDefault(x => x.GroupLINK == groupLink && x.ChannelLINK == channelLink);
			if (map == null)
			{
				map = new GroupChannelMap()
				{
					GroupLINK = groupLink,
					ChannelLINK = channelLink
				};

				_dataAdapter.SaveGroupMap(map);

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
			List<GroupChannelMap> maps = _dataAdapter.GetGroupMap();
			GroupChannelMap map = maps.SingleOrDefault(x => x.GroupLINK == groupLink && x.ChannelLINK == channelLink);
			if (map == null)
				return;

			GroupInfo defaultGroup = GetOrCreateDefaultGroup();

			GroupInfo[] groups = GetChannelGroups(channelLink);
			if (groups.Count() == 1)
			{
				if (force)
				{
					_dataAdapter.DeleteGroupMap(map);
				}
				else
				{
					if (groups[0].LINK != defaultGroup.LINK)
					{
						map.GroupLINK = defaultGroup.LINK;
						_dataAdapter.SaveGroupMap(map);
					}
				}
			}
			else if (groups.Count() > 1)
			{
				_dataAdapter.DeleteGroupMap(map);
			}

			RefreshGroups();
		}

		public GroupInfo GetOrCreateDefaultGroup()
		{
			string defaultGroupName = GroupInfo.DefaultGroupName;
			List<GroupInfo> groups = _groups.Values.ToList();

			GroupInfo deafaultGroup = groups.SingleOrDefault(group => group.Name == defaultGroupName);
			if (deafaultGroup == null)
			{
				groups = _dataAdapter.GetGroups();
				deafaultGroup = groups.SingleOrDefault(group => group.Name == defaultGroupName);
				if (deafaultGroup == null)
				{
					deafaultGroup = new GroupInfo(defaultGroupName);
					_dataAdapter.SaveGroup(deafaultGroup);
				}

				_groups.TryAdd(deafaultGroup.Name, deafaultGroup);
			}

			return deafaultGroup;
		}

		private void RefreshGroups()
		{
			List<GroupInfo> groups = _dataAdapter.GetGroups();
			_groups.Clear();
			foreach (GroupInfo group in groups)
			{
				_groups.TryAdd(group.Name, group);
			}
		}
		#endregion

	}
}
