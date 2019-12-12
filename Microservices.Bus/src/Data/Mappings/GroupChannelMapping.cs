using Microservices.Data.Mappings;

namespace Microservices.Bus.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public class GroupChannelMapping : ClassMapBase<DAO.GroupChannelMap>
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineTable()
		{
			Table(SysDatabase.Tables.GROUP_CHANNELS);
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
			Map(x => x.GroupLINK, "GROUP_LINK").UniqueKey("UQ_rms_GROUPCHANNEL").Index("IX_rms_GROUP_LINK");
			Map(x => x.ChannelLINK, "CHANNEL_LINK").UniqueKey("UQ_rms_GROUPCHANNEL").Index("IX_rms_CHANNEL_LINK");
		}
	}
}
