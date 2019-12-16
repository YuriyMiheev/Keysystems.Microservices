using System;
using System.Net.Mime;

namespace Microservices
{
	public static class MessageContentExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		public static ContentType ContentType(this MessageContent content)
		{
			#region Validate parameters
			if (content == null)
				throw new ArgumentNullException("content");
			#endregion

			return ContentType(content.Type, content.Name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		public static bool IsBase64(this MessageContent content)
		{
			#region Validate parameters
			if (content == null)
				throw new ArgumentNullException("content");
			#endregion

			if (String.IsNullOrWhiteSpace(content.Type))
				return false;

			return content.ContentType().IsBase64();
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
