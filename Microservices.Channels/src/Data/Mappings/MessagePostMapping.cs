using System;

using FluentNHibernate.Mapping;

namespace Keysystems.RemoteMessaging.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public class MessagePostMapping : ClassMapBase<DAO.MessagePost>
	{
		/// <summary>
		/// 
		/// </summary>
		protected override void DefineTable()
		{
			Table(MessageTables.MESSAGE_POSTS);
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
			//Map(x => x.MessageLINK, "MESSAGE_LINK").Index("IX_rms_MESSAGE_LINK");
			References(x => x.Message, "MESSAGE_LINK").Index("IX_rms_MESSAGE_LINK").Cascade.SaveUpdate();
			Map(x => x.Address, "ADDRESS").Length(255).Index("IX_rms_ADDRESS");
			Map(x => x.RetryCount, "RETRY");
			Component(x => x.Status);
		}
	}
}
