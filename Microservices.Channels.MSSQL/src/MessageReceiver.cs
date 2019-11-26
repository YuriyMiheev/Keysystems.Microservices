using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microservices.Channels.Configuration;
using Microservices.Channels.MSSQL.Adapters;

namespace Microservices.Channels.MSSQL
{
	/// <summary>
	/// 
	/// </summary>
	public class MessageReceiver : MessageReceiverBase
	{
		private MessageDataAdapter _dataAdapter;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="channelService"></param>
		public MessageReceiver(IChannelService channelService)
			: base(channelService)
		{
			_dataAdapter = (MessageDataAdapter)this.Channel.MessageDataAdapter;
		}
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="reliable"></param>
		/// <returns></returns>
		public override Message ReceiveMessage(Message msg)
		{
			#region Validate parameters
			if ( msg == null )
				throw new ArgumentNullException("msg");
			#endregion

			int? resLink;

			try
			{
				BeforeReceiveMessage(msg);
				resLink = CallReceiveMessageSP(msg);

				if ( msg.Async )
					msg.SetStatus(MessageStatus.DELIVERED);
				else
					msg.SetStatus(MessageStatus.COMPLETED);
			}
			catch ( Exception ex )
			{
				msg.SetStatus(MessageStatus.ERROR, ex.ToString());
				throw;
			}
			finally
			{
				this.Channel.SaveMessage(msg);
			}

			return ResponseMessage(msg, resLink);
		}
		#endregion


		#region Helpers
		private void BeforeReceiveMessage(Message msg)
		{
			MessageValidator.CheckReceiveMessage(this.Channel, msg);
			msg.SetStatus(MessageStatus.RECEIVING);
			this.Channel.SaveMessage(msg);
		}

		private int? CallReceiveMessageSP(Message msg)
		{
			int? resLink = null;

			DatabaseSettings databaseSettings = this.Channel.DatabaseSettings;
			if ( databaseSettings.ReceiveMessageSPEnabled )
			{
				string receiveMessageSP = databaseSettings.ReceiveMessageSP;
				if ( String.IsNullOrWhiteSpace(receiveMessageSP) )
					throw new ConfigSettingsException("Не задано имя хранимой процедуры приема сообщений.", "DATABASE.RECEIVE_SP");

				LogTrace(String.Format("Вызов хранимой процедуры \"{0}\" для сообщения {1}.", receiveMessageSP, msg));
				resLink = _dataAdapter.CallReceiveMessageSP(receiveMessageSP, msg, databaseSettings.ReceiveMessageSPUseOutputParam);
			}

			return resLink;
		}

		private Message ResponseMessage(Message inMsg, int? resLink)
		{
			if ( resLink == null || resLink == 0 )
				return null;

			Message resMsg = this.Channel.GetMessage(resLink.Value);
			PrepareResponseMessage(resMsg);
			resMsg.To = inMsg.From;

			this.Channel.SaveMessage(resMsg);

			try
			{
				if ( resLink <= inMsg.LINK )
					throw new MessageException(String.Format("Ответное сообщение #{0} должно быть следующим после принятого сообщения {1}.", resLink, inMsg));

				MessageValidator.CheckResponseMessage(this.Channel, resMsg);
			}
			catch ( Exception ex )
			{
				resMsg.SetStatus(MessageStatus.ERROR, ex.ToString());
				this.Channel.SaveMessage(resMsg);
			}

			CorrelateMessages(inMsg, resMsg);
			this.Channel.SaveMessage(inMsg);
			this.Channel.SaveMessage(resMsg);

			return this.Channel.GetMessage(resMsg.LINK);
		}
		#endregion

	}
}
