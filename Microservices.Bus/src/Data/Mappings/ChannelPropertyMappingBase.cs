using Microservices.Data.Mappings;

namespace Microservices.Bus.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class ChannelPropertyMappingBase : ClassMapBase<DAO.ChannelProperty>
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineTable()
		{
			Table(SysDatabase.Tables.CHANNEL_PROPERTIES);
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
			//Map(x => x.ChannelLINK, "CHANNEL_LINK").UniqueKey("UQ_rms_CHANNEL_NAME").Index("IX_rms_CHANNEL_LINK");
			References(x => x.Channel, "CHANNEL_LINK").UniqueKey("UQ_rms_CHANNEL_NAME").Index("IX_rms_CHANNEL_LINK").Cascade.SaveUpdate();
			Map(x => x.Name, "NAME").Length(255).Not.Nullable().UniqueKey("UQ_rms_CHANNEL_NAME").Index("IX_rms_NAME");
			Map(x => x.Type, "TYPE").Length(255);
			Map(x => x.Format, "FORMAT").Length(255);
			Map(x => x.Comment, "COMMENTS").Length(1024);
			Map(x => x.ReadOnly, "READONLY");
			Map(x => x.Secret, "SECRET");
			//Map(x => x.Value, "VALUE").Length(64 * 1024 * 1024);
			//Map(x => x.DefaultValue, "DEFAULT_VALUE").Length(64 * 1024 * 1024);
		}
	}
}
