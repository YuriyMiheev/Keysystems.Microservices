using Microservices.Data.Mappings;

namespace Microservices.Data.MSSQL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class ContactPropertyMapping : ContactPropertyMappingBase
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineColumns()
		{
			base.DefineColumns();
			Map(x => x.Value, "VALUE").CustomType("StringClob").CustomSqlType("nvarchar(max)");
		}
	}
}
