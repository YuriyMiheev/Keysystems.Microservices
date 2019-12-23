using System;
using System.Net.Mime;

namespace Microservices
{
	public static class MessageBodyInfoExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="bodyInfo"></param>
		/// <returns></returns>
		public static ContentType ContentType(this MessageBodyInfo bodyInfo)
		{
			#region Validate parameters
			if (bodyInfo == null)
				throw new ArgumentNullException("bodyInfo");
			#endregion

			return ContentType(bodyInfo.Type, bodyInfo.Name);
		}

		/// <summary>
		/// Содержимое закодировано в Base64.
		/// </summary>
		/// <returns></returns>
		public static bool IsBase64(this MessageBodyInfo bodyInfo)
		{
			#region Validate parameters
			if (bodyInfo == null)
				throw new ArgumentNullException("bodyInfo");
			#endregion

			if (String.IsNullOrWhiteSpace(bodyInfo.Type))
				return false;

			return bodyInfo.ContentType().IsBase64();
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
