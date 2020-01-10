using AutoMapper;

namespace Microservices.Bus.Web
{
	public static class ChannelDescriptionExtensions
	{
		private readonly static IMapper mapper;


		static ChannelDescriptionExtensions()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Addins.IAddinDescription, VMO.ChannelDescription>().ForMember(vmo => vmo.Properties, opt => opt.Ignore());
				cfg.CreateMap<Addins.AddinDescriptionProperty, VMO.ChannelDescriptionProperty>();
			});
			mapper = config.CreateMapper();
		}

		public static VMO.ChannelDescription ToVmo(this Addins.IAddinDescription obj)
		{
			var vmo = new VMO.ChannelDescription()
			{
				AllowMultipleInstances = obj.AllowMultipleInstances,
				CanSyncContacts = obj.CanSyncContacts,
				BinPath = obj.AddinPath,
				Comment = obj.Comment,
				IconName = obj.IconName,
				Provider = obj.Provider,
				RealAddress = obj.RealAddress,
				Timeout = obj.Timeout,
				Type = obj.Type,
				Version = obj.Version
			};
			//var vmo = mapper.Map<Addins.IAddinDescription, VMO.ChannelDescription>(obj);

			foreach (string propName in obj.Properties.Keys)
			{
				vmo.Properties.Add(obj.Properties[propName].ToVmo());
			}

			return vmo;
		}

		public static VMO.ChannelDescriptionProperty ToVmo(this Addins.AddinDescriptionProperty obj)
		{
			//var vmo = new VMO.ChannelDescriptionProperty()
			//{
			//	Comment = obj.Comment,
			//	DefaultValue = obj.DefaultValue,
			//	Format = obj.Format,
			//	Name = obj.Name,
			//	ReadOnly = obj.ReadOnly,
			//	Secret = obj.Secret,
			//	Type = obj.Type,
			//	Value = obj.Value
			//};

			var vmo = mapper.Map<Addins.AddinDescriptionProperty, VMO.ChannelDescriptionProperty>(obj);
			return vmo;
		}
	}
}
