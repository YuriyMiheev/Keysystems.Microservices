using System;

using FluentNHibernate.Mapping;
using Microservices.Data.Mappings;

namespace Microservices.Data.MSSQL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class MessageContentMapping : MessageContentMappingBase
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
