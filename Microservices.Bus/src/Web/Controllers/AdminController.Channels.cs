using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
					IAddinDescription description = _addinManager.FindDescription(channelInfo.Provider);
					ExceptionWrapper error = channelStatus.Error.Wrap();
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
					};

				}).ToList();

			var groups = channelGroups.Select(group =>
					new
					{
						group.LINK,
						group.Name,
						group.Image,
						Channels = channels.Where(ch => group.Channels.Contains(ch.LINK)).ToArray()
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
							desc.IconName,
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
							desc.IconName,
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
			if (channelLink == null || channelLink == 0)
			{
				ChannelInfo channelInfo = _channelManager.NewChannelTemplate(provider);
				this.ViewBag.Channel = channelInfo.ToVmo();
			}
			else
			{
				IChannelContext context = _channelManager.GetChannel(channelLink.Value);
				ChannelStatus channelStatus = context.Status;
				ChannelInfo channelInfo = context.Info;
				IAddinDescription description = _addinManager.FindDescription(channelInfo.Provider);

				this.ViewBag.Channel = channelInfo.ToVmo();
				this.ViewBag.Description = description.ToVmo();
				this.ViewBag.Status = channelStatus.ToVmo();
			}

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

			string contentType = MediaType.GetMimeByFileName(description.IconName);
			return File(data, contentType);
		}

		[AcceptVerbs("POST")]
		//[AdminAccess]
		//[NoAsyncTimeout]
		public async Task<IActionResult> OpenChannelAsync(int channelLink)
		{
			try
			{
				IChannelContext channelContext = _channelManager.GetChannel(channelLink);
				await _channelManager.StartChannelAsync(channelLink);

				ChannelInfo channelInfo = channelContext.Info;
				ChannelStatus channelStatus = channelContext.Status;
				IAddinDescription description = _addinManager.FindDescription(channelInfo.Provider);

				if (this.Request.IsAjaxRequest())
					return Json(new { success = true, Channel = channelInfo.ToVmo(), Description = description.ToVmo(), Status = channelStatus.ToVmo() });
				else
					return RedirectToAction("Channels");
			}
			catch (Exception ex)
			{
				if (this.Request.IsAjaxRequest())
					return Json(new { success = false, Error = ex.AllMessages() });

				throw;
			}
		}

		[AcceptVerbs("POST")]
		//[AdminAccess]
		//[NoAsyncTimeout]
		public async Task<IActionResult> CloseChannelAsync(int channelLink)
		{
			try
			{
				IChannelContext channelContext = _channelManager.GetChannel(channelLink);
				await channelContext.TerminateChannelAsync();

				ChannelInfo channelInfo = channelContext.Info;
				ChannelStatus channelStatus = channelContext.Status;
				IAddinDescription description = _addinManager.FindDescription(channelInfo.Provider);

				if (this.Request.IsAjaxRequest())
					return Json(new { success = true, Channel = channelInfo.ToVmo(), Description = description.ToVmo(), Status = channelStatus.ToVmo() });
				else
					return RedirectToAction("Channels");
			}
			catch (Exception ex)
			{
				if (this.Request.IsAjaxRequest())
					return Json(new { success = false, Error = ex.AllMessages() });

				throw;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channelLink"></param>
		/// <returns></returns>
		[AcceptVerbs("POST")]
		//[AdminAccess]
		public async Task<IActionResult> RunChannelAsync(int channelLink)
		{
			IChannelContext channelContext = _channelManager.GetChannel(channelLink);
			bool success;
			Exception error = null;

			try
			{
				await channelContext.Channel.RunAsync();
				success = true;
			}
			catch (Exception ex)
			{
				success = false;
				error = ex;
			}

			ChannelInfo channelInfo = channelContext.Info;
			ChannelStatus channelStatus = channelContext.Status;
			IAddinDescription description = _addinManager.FindDescription(channelInfo.Provider);

			if (this.Request.IsAjaxRequest())
				return Json(new { success, Error = error?.AllMessages(), Channel = channelInfo.ToVmo(), Description = description.ToVmo(), Status = channelStatus.ToVmo() });
			else
				return RedirectToAction("Channels");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channelLink"></param>
		[AcceptVerbs("POST")]
		//[AdminAccess]
		public async Task<IActionResult> StopChannelAsync(int channelLink)
		{
			IChannelContext channelContext = _channelManager.GetChannel(channelLink);
			bool success;
			Exception error = null;

			try
			{
				await channelContext.Channel.StopAsync();
				success = true;
			}
			catch (Exception ex)
			{
				success = false;
				error = ex;
			}

			ChannelInfo channelInfo = channelContext.Info;
			ChannelStatus channelStatus = channelContext.Status;
			IAddinDescription description = _addinManager.FindDescription(channelInfo.Provider);

			if (this.Request.IsAjaxRequest())
				return Json(new { success, Error = error?.AllMessages(), Channel = channelInfo.ToVmo(), Description = description.ToVmo(), Status = channelStatus.ToVmo() });
			else
				return RedirectToAction("Channels");
		}

		#region Helpers
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

			if (String.IsNullOrWhiteSpace(description.IconName))
				return false;

			string filePath = System.IO.Path.Combine(description.AddinPath, "wwwroot", description.IconName);
			return System.IO.File.Exists(filePath);
		}

		private byte[] LoadIconFile(IAddinDescription description)
		{
			if (!IconFileExist(description))
				return null;

			string filePath = System.IO.Path.Combine(description.AddinPath, "wwwroot", description.IconName);
			return System.IO.File.ReadAllBytes(filePath);
		}
		#endregion

	}
}
