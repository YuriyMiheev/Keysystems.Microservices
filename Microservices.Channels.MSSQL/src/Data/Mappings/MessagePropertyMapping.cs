using System;

using FluentNHibernate.Mapping;
using Microservices.Channels.Data.Mappings;

namespace Microservices.Channels.MSSQL.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class MessagePropertyMapping : MessagePropertyMappingBase
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
