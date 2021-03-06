﻿using Microservices;
using Microservices.Channels;
using Microservices.Channels.Data;
using Microservices.Channels.Logging;

using NHibernate.Criterion;

using DAO = Microservices.Data.DAO;

namespace MSSQL.Microservice
{
	/// <summary>
	/// 
	/// </summary>
	public class MessageScanner : DatabaseMessageScanner, IMessageScanner
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataAdapter"></param>
		/// <param name="logger"></param>
		public MessageScanner(IChannelDataAdapter dataAdapter, ILogger logger)
			: base(dataAdapter, logger)
		{ }
		#endregion


		#region Override
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override QueryOver<DAO.Message, DAO.Message> CreateOfflineSelectMessagesQuery()
		{
			var query = QueryOver.Of<DAO.Message>();
			//	query = query.Where(msg => msg.Channel == this.channel.VirtAddress);

			//if (this.Recipient != "*")
			//	query = query.Where(msg => msg.To == this.Recipient);

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
