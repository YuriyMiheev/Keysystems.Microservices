using System;
using System.Net.Mime;

namespace Microservices
{
	public static class MessageBodyExtensions
	{
		public static ContentType ContentType(this MessageBody body)
		{
			#region Validate parameters
			if (body == null)
				throw new ArgumentNullException(nameof(body));
			#endregion

			return ContentType(body.Type, body.Name);
		}

		/// <summary>
		/// Содержимое закодировано в Base64.
		/// </summary>
		/// <returns></returns>
		public static bool IsBase64(this MessageBody body)
		{
			#region Validate parameters
			if (body == null)
				throw new ArgumentNullException("body");
			#endregion

			if (String.IsNullOrWhiteSpace(body.Type))
				return false;

			return body.ContentType().IsBase64();
		}

		private static ContentType ContentType(string contentType, string name)
		{
			try
			{
				return new ContentType(contentType);
			}
			catch
			{
				string mime = MediaType.GetMimeByFileName(name);
				if (String.IsNullOrWhiteSpace(mime))
					return new ContentType("text/plain; charset=utf-8");
				else
					return new ContentType(mime);
			}
		}
	}
}
