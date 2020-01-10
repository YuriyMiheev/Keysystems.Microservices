
using AutoMapper;

namespace Microservices.Bus.Web
{
	public static class ChannelStatusExtensions
	{
		private readonly static IMapper mapper;


		static ChannelStatusExtensions()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Microservices.Channels.ChannelStatus, VMO.ChannelStatus>();
			});
			mapper = config.CreateMapper();
		}


		public static VMO.ChannelStatus ToVmo(this Microservices.Channels.ChannelStatus obj)
		{
			if (obj == null)
				return null;

			//var vmo = new VMO.ChannelStatus()
			//{
			//	Created = obj.Created,
			//	Error = obj.Error,
			//	Online = obj.Online,
			//	Opened = obj.Opened,
			//	Running = obj.Running
			//};

			var vmo = mapper.Map<Microservices.Channels.ChannelStatus, VMO.ChannelStatus>(obj);
			return vmo;
		}
	}
}
