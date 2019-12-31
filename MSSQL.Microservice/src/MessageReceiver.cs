using System;

using Microservices;
using Microservices.Channels;
using Microservices.Channels.Configuration;
using Microservices.Channels.Data;
using Microservices.Configuration;
using Microservices.Logging;

namespace MSSQL.Microservice
{
	/// <summary>
	/// 
	/// </summary>
	public class MessageReceiver : MessageReceiverBase
	{
		private readonly IChannelDataAdapter _dataAdapter;
		private readonly IAppSettingsConfig _appConfig;
		private readonly ILogger _logger;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataAdapter"></param>
		/// <param name="logger"></param>
		public MessageReceiver(IChannelDataAdapter dataAdapter, ILogger logger, IAppSettingsConfig appConfig)
			: base(dataAdapter, logger)
		{
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
			_appConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
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
				_dataAdapter.SaveMessage(msg);
			}

			return ResponseMessage(msg, resLink);
		}
		#endregion


		#region Helpers
		private void BeforeReceiveMessage(Message msg)
		{
			//MessageValidator.CheckReceiveMessage(this.Channel, msg);
			msg.SetStatus(MessageStatus.RECEIVING);
			_dataAdapter.SaveMessage(msg);
		}

		private int? CallReceiveMessageSP(Message msg)
		{
			int? resLink = null;

			DatabaseSettings databaseSettings = _appConfig.DatabaseSettings();
			if ( databaseSettings.ReceiveMessageSPEnabled )
			{
				string receiveMessageSP = databaseSettings.ReceiveMessageSP;
				if ( String.IsNullOrWhiteSpace(receiveMessageSP) )
					throw new ConfigSettingsException("Не задано имя хранимой процедуры приема сообщений.", "DATABASE.RECEIVE_SP");

				_logger.LogTrace(String.Format("Вызов хранимой процедуры \"{0}\" для сообщения {1}.", receiveMessageSP, msg));
				resLink = _dataAdapter.CallReceiveMessageSP(receiveMessageSP, msg, databaseSettings.ReceiveMessageSPUseOutputParam);
			}

			return resLink;
		}

		private Message ResponseMessage(Message inMsg, int? resLink)
		{
			if ( resLink == null || resLink == 0 )
				return null;

			Message resMsg = _dataAdapter.GetMessage(resLink.Value);
			PrepareResponseMessage(resMsg);
			resMsg.To = inMsg.From;

			_dataAdapter.SaveMessage(resMsg);

			try
			{
				if ( resLink <= inMsg.LINK )
					throw new MessageException(String.Format("Ответное сообщение #{0} должно быть следующим после принятого сообщения {1}.", resLink, inMsg));

				//MessageValidator.CheckResponseMessage(this.Channel, resMsg);
			}
			catch ( Exception ex )
			{
				resMsg.SetStatus(MessageStatus.ERROR, ex.ToString());
				_dataAdapter.SaveMessage(resMsg);
			}

			CorrelateMessages(inMsg, resMsg);
			_dataAdapter.SaveMessage(inMsg);
			_dataAdapter.SaveMessage(resMsg);

			return _dataAdapter.GetMessage(resMsg.LINK);
		}
		#endregion

	}
}
