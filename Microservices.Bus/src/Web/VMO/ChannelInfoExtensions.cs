using System.Linq;

using AutoMapper;
using Microservices.Bus.Channels;

namespace Microservices.Bus.Web
{
	public static class ChannelInfoExtensions
	{
		private readonly static IMapper mapper;


		static ChannelInfoExtensions()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Channels.ChannelInfo, VMO.ChannelInfo>().ForMember(vmo => vmo.Properties, opt => opt.Ignore());
				cfg.CreateMap<Channels.ChannelInfoProperty, VMO.ChannelInfoProperty>();
			});
			mapper = config.CreateMapper();
		}


		public static VMO.ChannelInfo ToVmo(this Channels.ChannelInfo obj)
		{
			if (obj == null)
				return null;

			//var vmo = new VMO.ChannelInfo()
			//{
			//	Comment = obj.Comment,
			//	Enabled = obj.Enabled,
			//	Id = obj.Id,
			//	IsSystem = obj.IsSystem(),
			//	LINK = obj.LINK,
			//	Name = obj.Name,
			//	PasswordIn = obj.PasswordIn,
			//	PasswordOut = obj.PasswordOut,
			//	Provider = obj.Provider,
			//	RealAddress = obj.RealAddress,
			//	SID = obj.SID,
			//	Timeout = obj.Timeout,
			//	VirtAddress = obj.VirtAddress
			//};

			var vmo = mapper.Map<Channels.ChannelInfo, VMO.ChannelInfo>(obj);
			vmo.IsSystem = obj.IsSystem();
			foreach (string propName in obj.Properties.Keys.OrderBy(propName => propName))
			{
				vmo.Properties.Add(obj.Properties[propName].ToVmo());
			}

			return vmo;
		}

		public static VMO.ChannelInfoProperty ToVmo(this Channels.ChannelInfoProperty obj)
		{
			//var vmo = new VMO.ChannelInfoProperty()
			//{
			//	ChannelLINK = obj.ChannelLINK,
			//	Comment = obj.Comment,
			//	Format = obj.Format,
			//	LINK = obj.LINK,
			//	Name = obj.Name,
			//	Type = obj.Type,
			//	Value = obj.Value
			//};

			var vmo = mapper.Map<Channels.ChannelInfoProperty, VMO.ChannelInfoProperty>(obj);
			return vmo;
		}
	}
}
