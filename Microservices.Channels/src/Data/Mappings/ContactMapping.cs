using System;

using FluentNHibernate.Mapping;

using Microservices.Data.Mappings;

namespace Microservices.Channels.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public class ContactMapping : ClassMapBase<DAO.Contact>
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineTable()
		{
			Table(Database.Tables.CONTACTS);
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
			Map(x => x.ContactID, "CONTACT_ID").Unique().Not.Nullable();
			Map(x => x.Type, "TYPE").Length(50);
			Map(x => x.Name, "NAME").Length(255);
			Map(x => x.Address, "ADDRESS").Length(255).Unique().Not.Nullable();
			Map(x => x.Enabled, "ENABLED");
			Map(x => x.IsService, "SERVICE");
			Map(x => x.IsMyself, "MYSELF");
			Map(x => x.Opened, "OPENED");
			Map(x => x.Online, "ON_LINE");
			Map(x => x.AccessMode, "ACCESS_MODE").Length(50);
			Map(x => x.Comment, "COMMENTS").Length(1024);
			HasMany(x => x.Properties).Table(Database.Tables.CONTACT_PROPERTIES).KeyColumn("CONTACT_LINK").ForeignKeyConstraintName("FKB7E49859C47832D1").Cascade.AllDeleteOrphan().Not.LazyLoad();
		}
	}
}
