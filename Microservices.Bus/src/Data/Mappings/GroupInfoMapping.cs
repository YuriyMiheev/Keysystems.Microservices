using Microservices.Data.Mappings;

namespace Microservices.Bus.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public class GroupInfoMapping : ClassMapBase<DAO.GroupInfo>
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineTable()
		{
			Table(SysDatabase.Tables.GROUPS);
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
			Map(x => x.Name, "NAME").Length(50).Index("IX_rms_NAME");
			Map(x => x.Image, "IMAGE").Length(50);
		}
	}
}
