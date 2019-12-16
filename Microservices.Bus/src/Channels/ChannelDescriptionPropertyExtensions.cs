using System;

using AutoMapper;

namespace Microservices.Bus.Channels
{
	public static class ChannelDescriptionPropertyExtensions
	{
		private readonly static IMapper mapper;

		static ChannelDescriptionPropertyExtensions()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<ChannelDescriptionProperty, ChannelProperty>();
			});
			mapper = config.CreateMapper();
		}

		public static void CopyTo(this ChannelDescriptionProperty src, ChannelProperty dest)
		{
			#region Validate parameters
			if (src == null)
				throw new ArgumentNullException("src");

			if (dest == null)
				throw new ArgumentNullException("dest");
			#endregion

			mapper.Map<ChannelDescriptionProperty, ChannelProperty>(src, dest);
		}
	}
}
