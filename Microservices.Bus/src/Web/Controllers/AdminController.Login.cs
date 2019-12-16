using System;

using Microsoft.AspNetCore.Mvc;

namespace Microservices.Bus.Web.Controllers
{
	partial class AdminController
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[AcceptVerbs("GET")]
		//[NoCache]
		public ActionResult Login()
		{
			this.ViewBag.Service = _serviceInfo.ToVmo();
			return View("Login");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[AcceptVerbs("GET")]
		//[NoCache]
		public ActionResult LoginContent()
		{
			this.ViewBag.Service = _serviceInfo.ToVmo();
			return PartialView("_LoginContent");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		[AcceptVerbs("POST")]
		public ActionResult Login(string userName, string password)
		{
			try
			{
				if (_busSettings.AuthorizationRequired)
				{
				}

				if (_serviceInfo.StartupError == null)
					return RedirectToAction("Channels");
				else
					return RedirectToAction("Home");
			}
			catch (Exception ex)
			{

				this.ViewBag.Service = _serviceInfo.ToVmo();
				this.ViewBag.UserName = userName;
				this.ViewBag.LoginError = ex.Wrap();
				return View("Login");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[AcceptVerbs("GET", "POST")]
		public ActionResult Logout()
		{
			return RedirectToAction("Login");
		}

		[AcceptVerbs("GET")]
		//[AdminAccess]
		//[NoCache]
		public ActionResult Home()
		{
			ServiceInfo serviceInfo = _serviceInfo;
			this.ViewBag.Service = serviceInfo.ToVmo();
			return View("Home");
		}
	}
}