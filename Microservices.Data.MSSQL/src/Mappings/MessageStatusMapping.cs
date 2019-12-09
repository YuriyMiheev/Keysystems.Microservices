using System;

using NHibernate;
using FluentNHibernate.Mapping;
using Microservices.Data.Mappings;

namespace Microservices.Data.MSSQL.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class MessageStatusMapping : MessageStatusMappingBase
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineColumns()
		{
			base.DefineColumns();
			Map(x => x.Info, "STATUS_INFO").CustomType("StringClob").CustomSqlType("nvarchar(max)");
		}
	}
}
