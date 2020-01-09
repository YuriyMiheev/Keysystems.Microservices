using System;
using System.Linq;
using System.Net;

using Microservices.Bus.Addins;
using Microservices.Bus.Channels;
using Microservices.Channels;

using Microsoft.AspNetCore.Mvc;

namespace Microservices.Bus.Web.Controllers
{
	partial class AdminController
	{
		[AcceptVerbs("GET")]
		//[AdminAccess]
		//[NoCache]
		public IActionResult Channels(int? groupLink)
		{
			GroupInfo[] channelGroups = _channelManager.ChannelsGroups;
			IChannelContext[] runtimeChannels = _channelManager.RuntimeChannels;

			if (groupLink == null)
				groupLink = GetGroupLink();

			var channels = runtimeChannels.Select(context =>
				{
					ChannelInfo channelInfo = context.Info;
					ChannelStatus channelStatus = context.Status;
					IChannel channel = context.Channel;
					ExceptionWrapper error = context.Status.Error.Wrap();
					IAddinDescription description = _addinManager.FindDescription(channelInfo.Provider);
					return new
					{
						channelInfo.LINK,
						channelInfo.Name,
						channelInfo.Provider,
						channelInfo.VirtAddress,
						channelInfo.SID,
						channelInfo.RealAddress,
						channelInfo.Timeout,
						IsSystem = channelInfo.IsSystem(),
						channelInfo.Enabled,
						channelInfo.Comment,
						channelStatus.Opened,
						channelStatus.Running,
						channelStatus.Online,
						description.CanSyncContacts,
						LastError = (error != null ? error.Time.Value.ToString("[dd.MM.yyyy HH:mm:ss]") + ' ' + error.Message.Split('\n')[0] : ""),
						//IconCss
					};
				}).ToList();

			var groups = channelGroups.Select(group =>
					new
					{
						group.LINK,
						group.Name,
						group.Image,
						Channels = channels.Where(channelInfo => group.Channels.Contains(channelInfo.LINK)).ToArray()
					}
				).ToList();

			if (this.Request.IsAjaxRequest())
			{
				if (groupLink != null)
				{
					var group = groups.Single(g => g.LINK == groupLink);
					return Json(group.Channels);
				}
				else
				{
					return Json(channels);
				}
			}
			else
			{
				if (_serviceInfo.StartupError != null)
					return RedirectToAction("Home");

				bool systemExist = runtimeChannels.Any(context => context.Info.IsSystem());
				var registeredChannels = _addinManager.RegisteredChannels.Select(desc =>
						new
						{
							desc.Provider,
							desc.Comment,
							Disabled = (desc.Provider == "SYSTEM" && systemExist || desc.Provider != "SYSTEM" && !systemExist)
						}
					).ToList();

				this.ViewBag.ChannelGroups = groups;
				this.ViewBag.RegisteredChannels = registeredChannels;
				return View("Channels");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[AcceptVerbs("GET")]
		//[AdminAccess]
		//[NoCache]
		public IActionResult RegisteredChannels()
		{
			try
			{
				IChannelContext[] runtimeChannels = _channelManager.RuntimeChannels;
				bool systemExist = runtimeChannels.Any(context => context.Info.IsSystem());

				var registeredChannels = _addinManager.RegisteredChannels.Select(desc =>
						new
						{
							desc.Provider,
							//IconCss
							IconName = desc.Icon,
							//desc.IsolatedDomain,
							desc.Comment,
							Disabled = (desc.Provider == "SYSTEM" && systemExist || desc.Provider != "SYSTEM" && !systemExist)
						}
					).ToList();
				return Json(registeredChannels);
			}
			catch (Exception ex)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, ex.AllMessages());
			}
		}

		/// <summary>
		/// Просмотр канала.
		/// </summary>
		/// <param name="channelLink"></param>
		/// <param name="provider"></param>
		/// <returns></returns>
		[AcceptVerbs("GET")]
		//[AdminAccess]
		//[NoCache]
		public IActionResult Channel(int? channelLink, string provider)
		{
			ChannelInfo channelInfo;

			if (channelLink == null || channelLink == 0)
			{
				channelInfo = _channelManager.NewChannelTemplate(provider);
			}
			else
			{
				IChannelContext channel = _channelManager.GetChannel(channelLink.Value);
				channelInfo = channel.Info;
			}

			this.ViewBag.Channel = channelInfo.ToVmo();
			return View("Channel");
		}

		[AcceptVerbs("GET", "POST")]
		//[AdminAccess]
		public IActionResult ChannelIcon(string provider)
		{
			IAddinDescription description = _addinManager.FindDescription(provider);
			byte[] data = LoadIconFile(description);
			if (data == null)
				return null;

			string contentType = MediaType.GetMimeByFileName(description.Icon);
			return File(data, contentType);
		}

		private int? GetGroupLink()
		{
			int? result = null;

			foreach (var cookie in this.Request.Cookies)
			{
				if (cookie.Key == "groupLink")
				{
					int groupLink;
					if (Int32.TryParse(cookie.Value, out groupLink))
					{
						if (_channelManager.ChannelsGroups.Any(group => group.LINK == groupLink))
							result = groupLink;
					}
				}
			}

			return result;
		}

		private bool IconFileExist(IAddinDescription description)
		{
			if (description == null)
				return false;

			if (String.IsNullOrWhiteSpace(description.AddinPath))
				return false;

			if (String.IsNullOrWhiteSpace(description.Icon))
				return false;

			string filePath = System.IO.Path.Combine(description.AddinPath, "wwwroot", description.Icon);
			return System.IO.File.Exists(filePath);
		}

		private byte[] LoadIconFile(IAddinDescription description)
		{
			if (!IconFileExist(description))
				return null;

			string filePath = System.IO.Path.Combine(description.AddinPath, "wwwroot", description.Icon);
			return System.IO.File.ReadAllBytes(filePath);
		}
	}
}
