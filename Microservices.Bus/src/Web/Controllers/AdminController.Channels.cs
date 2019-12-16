using System;
using System.Linq;

using Microservices.Bus.Channels;

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
			ChannelInfo[] runtimeChannels = _channelManager.RuntimeChannels;

			if (groupLink == null)
				groupLink = GetGroupLink();

			var channels = runtimeChannels.Select(channelInfo =>
				new
				{
					channelInfo.LINK,
					channelInfo.Name,
					channelInfo.Provider,
					channelInfo.VirtAddress,
					channelInfo.SID,
					channelInfo.RealAddress,
					channelInfo.Timeout,
					channelInfo.IsSystem,
					channelInfo.Enabled,
					channelInfo.Comment,
					channelInfo.Opened,
					channelInfo.Running,
					channelInfo.Online,
					channelInfo.CanSyncContacts,
					LastError = (channelInfo.Error != null ? channelInfo.Error.Time.Value.ToString("[dd.MM.yyyy HH:mm:ss]") + ' ' + channelInfo.Error.Message.Split('\n')[0] : ""),
					//IconCss = (string)null //(IconFileExist(channelInfo.Description) ? null : (channelInfo.Description != null ? channelInfo.Description.IconCss : null))
				}
			).ToList();

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

				bool systemExist = runtimeChannels.Any(channelInfo => channelInfo.IsSystem);
				var registeredChannels = _addinManager.RegisteredChannels.Select(desc =>
						new
						{
							desc.Provider,
							//IconCss = (string)null, //(IconFileExist(desc) ? null : desc.IconCss),
							desc.Comment,
							Disabled = (desc.Provider == "SYSTEM" && systemExist || desc.Provider != "SYSTEM" && !systemExist)
						}
					).ToList();

				this.ViewBag.ChannelGroups = groups;
				this.ViewBag.RegisteredChannels = registeredChannels;
				return View("Channels");
			}
		}


		private int? GetGroupLink()
		{
			int? result = null;

			foreach(var cookie in this.Request.Cookies)
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
	}
}
