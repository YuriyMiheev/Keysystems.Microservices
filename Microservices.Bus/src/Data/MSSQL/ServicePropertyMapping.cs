﻿using Microservices.Bus.Data.Mappings;

namespace Microservices.Bus.Data.MSSQL
{
	/// <summary>
	/// 
	/// </summary>
	public class ServicePropertyMapping : ServicePropertyMappingBase
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineColumns()
		{
			base.DefineColumns();
			Map(x => x.Value, "VALUE").CustomType("StringClob").CustomSqlType("nvarchar(max)");
			Map(x => x.DafaultValue, "DEFAULT_VALUE").CustomType("StringClob").CustomSqlType("nvarchar(max)");
		}
	}
}
