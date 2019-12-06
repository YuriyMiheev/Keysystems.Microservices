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
	public static class MessageExtensions
	{

		#region ContentType
		/// <summary>
		/// 
		/// </summary>
		/// <param name="body"></param>
		/// <returns></returns>
		public static ContentType ContentType(this MessageBody body)
		{
			#region Validate parameters
			if (body == null)
				throw new ArgumentNullException("body");
			#endregion

			return ContentType(body.Type, body.Name);
		}

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

		private static ContentType ContentType(string contentType, string name)
		{
			if (String.IsNullOrWhiteSpace(contentType))
			{
				string mime = MediaType.GetMimeByFileName(name);
				if (String.IsNullOrWhiteSpace(mime))
					return new ContentType("text/plain; charset=utf-8");
				else
					return new ContentType(mime);
			}
			else
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
		#endregion


		#region IsBase64
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
		#endregion


		#region IsEmpty
		///// <summary>
		///// 
		///// </summary>
		///// <param name="body"></param>
		///// <returns></returns>
		//public static bool IsEmpty(this MessageBody body)
		//{
		//   #region Validate parameters
		//   if ( body == null )
		//      throw new ArgumentNullException("body");
		//   #endregion
		//   return body.BodyInfo().IsEmpty() && (body.Value == null);
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="bodyInfo"></param>
		///// <returns></returns>
		//public static bool IsEmpty(this MessageBodyInfo bodyInfo)
		//{
		//   #region Validate parameters
		//   if ( bodyInfo == null )
		//      throw new ArgumentNullException("bodyInfo");
		//   #endregion

		//   return (bodyInfo.MessageLINK == 0) && (bodyInfo.Name == null) && (bodyInfo.Type == null) && (bodyInfo.Length == null) && (bodyInfo.FileSize == null);
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="content"></param>
		///// <returns></returns>
		//public static bool IsEmpty(this MessageContent content)
		//{
		//   #region Validate parameters
		//   if ( content == null )
		//      throw new ArgumentNullException("content");
		//   #endregion

		//   return content.ContentInfo().IsEmpty() && (content.Value == null);
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="contentInfo"></param>
		///// <returns></returns>
		//public static bool IsEmpty(this MessageContentInfo contentInfo)
		//{
		//   #region Validate parameters
		//   if ( contentInfo == null )
		//      throw new ArgumentNullException("contentInfo");
		//   #endregion

		//   return (contentInfo.Name == null) && (contentInfo.Type == null) && (contentInfo.Length == null) && (contentInfo.FileSize == null) && (contentInfo.Comment == null);
		//}
		#endregion

	}
}
