using Microservices.Data.Mappings;

namespace Microservices.Bus.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public class ChannelInfoMapping : ClassMapBase<DAO.ChannelInfo>
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineTable()
		{
			Table(BusDatabase.Tables.CHANNELS);
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
			Map(x => x.Name, "NAME").Length(255).Index("IX_rms_NAME");
			Map(x => x.Provider, "PROVIDER").Length(50).Not.Nullable().Index("IX_rms_PROVIDER");
			Map(x => x.VirtAddress, "VIRT_ADDRESS").Length(255).Unique().Not.Nullable();
			Map(x => x.IsolatedDomain, "ISOLATED");
			Map(x => x.SID, "SID").Length(255).Index("IX_rms_SID");
			Map(x => x.RealAddress, "REAL_ADDRESS").Length(1024).Index("IX_rms_ADDRESS");
			Map(x => x.PasswordIn, "PASSWORD_IN").Length(255);
			Map(x => x.PasswordOut, "PASSWORD_OUT").Length(255);
			Map(x => x.Timeout, "TIMEOUT");
			Map(x => x.Enabled, "ENABLED").Index("IX_rms_ENABLED");
			Map(x => x.AccessMode, "ACCESS_MODE").Length(50).Index("IX_rms_ACCESS_MODE");
			Map(x => x.Comment, "COMMENTS").Length(1024);
			HasMany(x => x.Properties).Table(BusDatabase.Tables.CHANNEL_PROPERTIES).KeyColumn("CHANNEL_LINK").ForeignKeyConstraintName("FK8522B34FFB71BD91").Cascade.AllDeleteOrphan().Not.LazyLoad();
		}
	}
}
