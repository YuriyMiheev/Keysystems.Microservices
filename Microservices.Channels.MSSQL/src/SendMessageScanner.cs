using System.Collections.Generic;
using System.Linq;

//using Microservices.Channels.MSSQL.Adapters;
using NHibernate;
using NHibernate.Criterion;

//using Keysystems.RemoteMessaging.Adapters;
//using Keysystems.RemoteMessaging.Data;

namespace Microservices.Channels.MSSQL
{
	/// <summary>
	/// 
	/// </summary>
	public class SendMessageScanner : MessageScannerBase
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="recipient"></param>
		public SendMessageScanner(IChannelService channelService, string recipient)
			: base(channelService, recipient)
		{ }
		#endregion


		#region Override
		/// <summary>
		/// 
		/// </summary>
		/// <param name="exceptLinks"></param>
		/// <returns></returns>
		protected override QueryOver<DAO.Message, DAO.Message> CreateOfflineSelectMessagesQuery()
		{
			var query = QueryOver.Of<DAO.Message>();
			//	query = query.Where(msg => msg.Channel == this.channel.VirtAddress);

			if ( this.Recipient != "*" )
				query = query.Where(msg => msg.To == this.Recipient);

			query = query
				.Where(msg => msg.Direction == MessageDirection.OUT)
				.Where(msg => msg.Class == null || msg.Class == "" || msg.Class == MessageClass.REQUEST || msg.Class == MessageClass.RESPONSE)
				.Where(msg => msg.Status.Value == MessageStatus.NEW)
				//.WhereRestrictionOn(msg => msg.LINK).Not.IsIn(exceptLinks.ToArray())
				.OrderBy(msg => msg.Priority).Desc
				.ThenBy(msg => msg.LINK).Asc;

			return query;
		}
		#endregion

	}
}
