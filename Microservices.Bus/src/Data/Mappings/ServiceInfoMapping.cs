using Microservices.Data.Mappings;

namespace Microservices.Bus.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public class ServiceInfoMapping : ClassMapBase<DAO.ServiceInfo>
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineTable()
		{
			Table(SysDatabase.Tables.SERVICE);
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
			Map(x => x.InstanceID, "INSTANCE_ID").Length(255);
			Map(x => x.ServiceName, "SERVICE_NAME").Length(255);
			Map(x => x.Version, "VERSION").Length(255);
			Map(x => x.StartTime, "START_TIME");
			Map(x => x.ShutdownTime, "SHUTDOWN_TIME");
			Map(x => x.ShutdownReason, "SHUTDOWN_REASON").Length(255);
			Map(x => x.ExternalAddress, "EXTERNAL_ADDRESS").Length(255);
			Map(x => x.InternalAddress, "INTERNAL_ADDRESS").Length(255);
			Map(x => x.DebugEnabled, "DEBUG");
			Map(x => x.AuthorizeEnabled, "AUTHORIZE");
			Map(x => x.MaxUploadSize, "MAX_UPLOAD_SIZE");
			Map(x => x.Online, "ON_LINE");
			HasMany(x => x.Properties).Table(SysDatabase.Tables.SERVICE_PROPERTIES).KeyColumn("SERVICE_LINK").ForeignKeyConstraintName("FK4336FD888A8ED41F").Cascade.AllDeleteOrphan().Not.LazyLoad();
		}
	}
}
