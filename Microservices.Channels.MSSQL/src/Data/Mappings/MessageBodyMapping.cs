using System;

using FluentNHibernate.Mapping;
using Microservices.Channels.Data.Mappings;

namespace Microservices.Channels.MSSQL.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class MessageBodyMapping : MessageBodyMappingBase
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineColumns()
		{
			base.DefineColumns();
			Map(x => x.Value, "BODY_VALUE").CustomType("StringClob").CustomSqlType("nvarchar(max)");
		}
	}
}
