using System;

using FluentNHibernate.Mapping;

namespace Microservices.Channels.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public class DateStatMessageMapping : ClassMapBase<DAO.DateStatMessage>
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
			Map(x => x.Channel, "CHANNEL");
			Map(x => x.Date, "DATE_TIME");
			Map(x => x.Status, "STATUS");
		}
	}
}
