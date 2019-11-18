using System;

using FluentNHibernate.Mapping;

namespace Microservices.Channels.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class ContactPropertyMappingBase : ClassMapBase<DAO.ContactProperty>
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineTable()
		{
			Table(Database.Tables.CONTACT_PROPERTIES);
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
			//Map(x => x.ContactLINK, "CONTACT_LINK").UniqueKey("UQ_rms_CONTACT_NAME").Index("IX_rms_CONTACT_LINK");
			References(x => x.Contact, "CONTACT_LINK").UniqueKey("UQ_rms_CONTACT_NAME").Index("IX_rms_CONTACT_LINK").Cascade.SaveUpdate();
			Map(x => x.Name, "NAME").Length(255).Not.Nullable().UniqueKey("UQ_rms_CONTACT_NAME").Index("IX_rms_NAME");
			Map(x => x.Type, "TYPE").Length(255);
			Map(x => x.Format, "FORMAT").Length(255);
			Map(x => x.Comment, "COMMENTS").Length(1024);
			//Map(x => x.Value, "VALUE").Length(64 * 1024 * 1024);
		}
	}
}
