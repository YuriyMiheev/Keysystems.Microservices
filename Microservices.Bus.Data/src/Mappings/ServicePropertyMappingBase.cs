using Microservices.Data.Mappings;

namespace Microservices.Bus.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class ServicePropertyMappingBase : ClassMapBase<DAO.ServiceProperty>
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineTable()
		{
			Table(SysDatabase.Tables.SERVICE_PROPERTIES);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void DefineKey()
		{
			Id(x => x.LINK, "LINK");
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void DefineColumns()
		{
			//Map(x => x.ServiceLINK, "SERVICE_LINK");
			References(x => x.Service, "SERVICE_LINK").Cascade.SaveUpdate();
			Map(x => x.Name, "NAME").Length(255).Not.Nullable().Unique();
			Map(x => x.Type, "TYPE").Length(255);
			Map(x => x.Format, "FORMAT").Length(255);
			Map(x => x.Comment, "COMMENTS").Length(1024);
			Map(x => x.ReadOnly, "READONLY");
			Map(x => x.Secret, "SECRET");
			//Map(x => x.Value, "VALUE").Length(64 * 1024 * 1024);
			//Map(x => x.DafaultValue, "DEFAULT_VALUE").Length(64 * 1024 * 1024);
		}
	}
}
