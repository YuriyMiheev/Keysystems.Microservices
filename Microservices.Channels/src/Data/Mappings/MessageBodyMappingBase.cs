using System;

using FluentNHibernate.Mapping;

namespace Microservices.Channels.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class MessageBodyMappingBase : ClassMapBase<DAO.MessageBody>
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
			//Map(x => x.Value, "BODY_VALUE").Length(64 * 1024 * 1024);
		}
	}
}
