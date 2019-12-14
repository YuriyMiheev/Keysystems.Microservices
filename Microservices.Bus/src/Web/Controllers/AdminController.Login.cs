using System;

using Microservices.Bus.Configuration;

using Microsoft.AspNetCore.Mvc;

namespace Microservices.Bus.Controllers
{
	public partial class AdminController : Controller
    {
		private readonly ServiceInfo _serviceInfo;
		private readonly BusSettings _busSettings;


		public AdminController(ServiceInfo serviceInfo, BusSettings busSettings)
		{
			_serviceInfo = serviceInfo ?? throw new ArgumentNullException(nameof(serviceInfo));
			_busSettings = busSettings ?? throw new ArgumentNullException(nameof(busSettings));
		}

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
				this.ViewBag.LoginError = ex;
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
	}
}