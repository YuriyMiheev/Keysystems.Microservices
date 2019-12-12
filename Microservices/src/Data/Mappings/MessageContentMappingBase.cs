using System;

using FluentNHibernate.Mapping;

namespace Microservices.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class MessageContentMappingBase : ClassMapBase<DAO.MessageContent>
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineTable()
		{
			Table(Database.Tables.MESSAGE_CONTENTS);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void DefineKey()
		{
			Id(x => x.LINK, "LINK");
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void DefineColumns()
		{
			//Map(x => x.MessageLINK, "MESSAGE_LINK").UniqueKey("UQ_rms_MESSAGE_NAME").Index("IX_rms_MESSAGE_LINK");
			References(x => x.Message, "MESSAGE_LINK").UniqueKey("UQ_rms_MESSAGE_NAME").Index("IX_rms_MESSAGE_LINK").Cascade.SaveUpdate();
			Map(x => x.Name, "NAME").Not.Nullable().Length(255).UniqueKey("UQ_rms_MESSAGE_NAME").Index("IX_rms_NAME");
			Map(x => x.Type, "TYPE").Length(255);
			Map(x => x.Length, "LENGTH");
			Map(x => x.FileSize, "FILE_SIZE");
			Map(x => x.Comment, "COMMENTS").Length(1024);
			//Map(x => x.Value, "VALUE").Length(64 * 1024 * 1024);
		}
	}
}
