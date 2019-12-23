using System;

using FluentNHibernate.Mapping;

namespace Microservices.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public class MessageBodyInfoMapping : ClassMapBase<DAO.MessageBodyInfo>
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineTable()
		{
			Table(Database.Tables.MESSAGES);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void DefineKey()
		{
			Id(x => x.MessageLINK, "LINK");
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void DefineColumns()
		{
			Map(x => x.Name, "BODY_NAME").Length(255);
			Map(x => x.Type, "BODY_TYPE").Length(255);
			Map(x => x.Length, "BODY_LENGTH");
			Map(x => x.FileSize, "BODY_FILESIZE");
		}
	}
}
