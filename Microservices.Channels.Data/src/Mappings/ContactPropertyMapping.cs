using System;

using FluentNHibernate.Mapping;
using Microservices.Channels.Data.Mappings;

namespace Microservices.Channels.Data.MSSQL.Mappings
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
