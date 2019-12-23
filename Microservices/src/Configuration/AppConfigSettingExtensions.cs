using System;

using AutoMapper;

namespace Microservices.Configuration
{
	public static class AppConfigSettingExtensions
	{
		private readonly static IMapper mapper;

		static AppConfigSettingExtensions()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<AppConfigSetting, AppConfigSetting>();
			});
			mapper = config.CreateMapper();
		}


		public static void CopyTo(this AppConfigSetting src, AppConfigSetting dest)
		{
			#region Validate parameters
			if (src == null)
				throw new ArgumentNullException(nameof(src));

			if (dest == null)
				throw new ArgumentNullException(nameof(dest));
			#endregion

			mapper.Map<AppConfigSetting, AppConfigSetting>(src, dest);
		}

	}
}
