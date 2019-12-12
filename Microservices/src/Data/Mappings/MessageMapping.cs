using System;

using FluentNHibernate.Mapping;

namespace Microservices.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public class MessageMapping : ClassMapBase<DAO.Message>
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
			Id(x => x.LINK, "LINK");
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void DefineColumns()
		{
			Map(x => x.Async, "ASYNC").Index("IX_rms_ASYNC");
			Map(x => x.Channel, "CHANNEL").Length(255).UniqueKey("UQ_rms_CHANNEL_GUID_DIRECTION").Index("IX_rms_CHANNEL");
			Map(x => x.Class, "CLASS").Length(50).Index("IX_rms_CLASS");
			Map(x => x.CorrGUID, "CORR_GUID").Length(255).Index("IX_rms_CORR_GUID");
			Map(x => x.CorrLINK, "CORR_LINK").Index("IX_rms_CORR_LINK");
			Map(x => x.Date, "DATE_TIME").Index("IX_rms_DATE_TIME");
			Map(x => x.Direction, "DIRECTION").Length(50).Not.Nullable().UniqueKey("UQ_rms_CHANNEL_GUID_DIRECTION").Index("IX_rms_DIRECTION");
			Map(x => x.From, "SENDER").Length(255).Index("IX_rms_SENDER");
			Map(x => x.GUID, "GUID").Length(255).Not.Nullable().UniqueKey("UQ_rms_CHANNEL_GUID_DIRECTION").Index("IX_rms_GUID");
			Map(x => x.Name, "NAME").Length(255).Index("IX_rms_NAME");
			Map(x => x.Priority, "PRIORITY").Index("IX_rms_PRIORITY");
			Map(x => x.Proxy, "PROXY").Length(2000).Index("IX_rms_PROXY");
			//Map(x => x.Queue, "QUEUE").Length(50).Index("IX_rms_QUEUE");
			Map(x => x.Subject, "SUBJECT").Length(1024).Index("IX_rms_SUBJECT");
			Map(x => x.To, "RECIPIENTS").Length(2000).Index("IX_rms_RECIPIENTS");
			Map(x => x.TTL, "TTL");
			Map(x => x.Type, "TYPE").Length(50).Index("IX_rms_TYPE");
			Map(x => x.Version, "VERSION").Length(50).Index("IX_rms_VERSION");

			Component(x => x.Body);
			HasMany(x => x.Contents).Table(Database.Tables.MESSAGE_CONTENTS).KeyColumn("MESSAGE_LINK").ForeignKeyConstraintName("FK8C78E7BFC6BE636C").Cascade.AllDeleteOrphan().Not.LazyLoad();
			//HasMany(x => x.Posts).Table(Database.Tables.MESSAGE_POSTS).KeyColumn("MESSAGE_LINK").ForeignKeyConstraintName("FK60670E5DC6BE636C").Cascade.AllDeleteOrphan().Not.LazyLoad();
			HasMany(x => x.Properties).Table(Database.Tables.MESSAGE_PROPERTIES).KeyColumn("MESSAGE_LINK").ForeignKeyConstraintName("FK3C00DD87C6BE636C").Cascade.AllDeleteOrphan().Not.LazyLoad();
			Component(x => x.Status);
		}
	}
}
