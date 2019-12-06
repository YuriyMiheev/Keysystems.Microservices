using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace Microservices
{
	/// <summary>
	/// 
	/// </summary>
	public static class ContentTypeExtensions
	{
		private static List<string> textMimes = new List<string>()
			{
				null,
				"",
				"text/plain",
				"text/xml",
				"text/css",
				"text/html",
				"text/javascript",
				"application/json",
				"application/x-javascript",
				"application/xml",
				"application/soap+xml"
			};

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentType"></param>
		/// <returns></returns>
		public static bool MustBase64(this ContentType contentType)
		{
			if ( contentType == null )
				throw new ArgumentNullException("contentType");

			return !textMimes.Contains(contentType.MediaType, StringComparer.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentType"></param>
		/// <returns></returns>
		public static bool IsBase64(this ContentType contentType)
		{
			if ( contentType == null )
				return false;

			if ( !contentType.Parameters.ContainsKey("Content-Transfer-Encoding") )
				return false;

			return (contentType.Parameters["Content-Transfer-Encoding"] == "base64");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentType"></param>
		public static void MarkBase64(this ContentType contentType)
		{
			if ( contentType == null )
				throw new ArgumentNullException("contentType");

			if ( !contentType.IsBase64() )
				contentType.Parameters.Add("Content-Transfer-Encoding", "base64");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentType"></param>
		/// <returns></returns>
		public static Encoding Encoding(this ContentType contentType)
		{
			Encoding encoding = System.Text.Encoding.UTF8;
			if ( !String.IsNullOrWhiteSpace(contentType.CharSet) )
				encoding = System.Text.Encoding.GetEncoding(contentType.CharSet);

			return encoding;
		}
	}
}
