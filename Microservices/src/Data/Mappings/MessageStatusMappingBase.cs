using System;

using FluentNHibernate.Mapping;

namespace Microservices.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class MessageStatusMappingBase : ComponentMapBase<DAO.MessageStatus>
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineColumns()
		{
			Map(x => x.Value, "STATUS").Length(50).Index("IX_rms_STATUS");
			Map(x => x.Date, "STATUS_DATE");
			Map(x => x.Code, "STATUS_CODE");
		}
	}
}
