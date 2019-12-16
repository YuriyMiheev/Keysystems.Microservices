using System;
using Microsoft.AspNetCore.Http;

namespace Microservices.Bus.Web
{
	public static class HttpRequestExtensions
	{
		public static bool IsAjaxRequest(this HttpRequest httpRequest)
		{
			if (httpRequest == null)
				throw new ArgumentNullException(nameof(httpRequest));

			//if (httpRequest.Headers["X-Requested-With"] == "XMLHttpRequest")
			//	return true;

			if (httpRequest.Headers != null)
				return httpRequest.Headers["X-Requested-With"] == "XMLHttpRequest";

			return false;
		}
	}
}
