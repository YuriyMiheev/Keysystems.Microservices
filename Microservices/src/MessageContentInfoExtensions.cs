using System;
using System.Net.Mime;

namespace Microservices
{
	public static class MessageContentInfoExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentInfo"></param>
		/// <returns></returns>
		public static ContentType ContentType(this MessageContentInfo contentInfo)
		{
			#region Validate parameters
			if (contentInfo == null)
				throw new ArgumentNullException("contentInfo");
			#endregion

			return ContentType(contentInfo.Type, contentInfo.Name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentInfo"></param>
		/// <returns></returns>
		public static bool IsBase64(this MessageContentInfo contentInfo)
		{
			#region Validate parameters
			if (contentInfo == null)
				throw new ArgumentNullException("contentInfo");
			#endregion

			if (String.IsNullOrWhiteSpace(contentInfo.Type))
				return false;

			return contentInfo.ContentType().IsBase64();
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
