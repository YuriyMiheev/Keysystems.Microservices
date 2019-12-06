using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.StaticFiles;

namespace Microservices
{
	public static class MediaType
	{
		static IContentTypeProvider _mimeTypes;

		static MediaType()
		{
			_mimeTypes = new FileExtensionContentTypeProvider();
		}


		public static string GetMimeByFileName(string fileName)
		{
			if (_mimeTypes.TryGetContentType(fileName, out string contentType))
				return contentType;
			else
				return null;
		}
	}
}
