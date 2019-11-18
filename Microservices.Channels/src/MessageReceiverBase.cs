using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using WCF = System.ServiceModel.Channels;
//using Keysystems.RemoteMessaging.Lib;
//using Keysystems.RemoteMessaging.Lib.Mime;

namespace Microservices.Channels
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class MessageReceiverBase //<TChannelServcie> where TChannelServcie : IChannelService
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		protected MessageReceiverBase(IChannelService channel)
		{
			#region Validate parameters
			if ( channel == null )
				throw new ArgumentNullException("channel");
			#endregion

			this.Channel = channel;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public IChannelService Channel { get; private set; }
		#endregion


		#region Methods
		/// <summary>
		/// Принять сообщение.
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public abstract Message ReceiveMessage(Message msg);

		/// <summary>
		/// Подготовить ответное сообщение.
		/// </summary>
		/// <param name="resMsg"></param>
		protected void PrepareResponseMessage(Message resMsg)
		{
			#region Validate parameters
			if ( resMsg == null )
				throw new ArgumentNullException("msg");
			#endregion

			LogTrace(String.Format("Подготовка ответного сообщения {0}.", resMsg));

			#region Header
			resMsg.Channel = this.Channel.VirtAddress;

			if ( resMsg.LINK == 0 )
				resMsg.SetStatus(MessageStatus.NULL);

			if ( String.IsNullOrWhiteSpace(resMsg.Direction) )
				resMsg.Direction = MessageDirection.OUT;

			if ( String.IsNullOrWhiteSpace(resMsg.From) )
				resMsg.From = this.Channel.VirtAddress;

			if ( String.IsNullOrWhiteSpace(resMsg.Version) )
				resMsg.Version = MessageVersion.Current;

			if ( String.IsNullOrWhiteSpace(resMsg.Type) )
				resMsg.Type = MessageType.DOCUMENT;

			if ( String.IsNullOrWhiteSpace(resMsg.Class) )
				resMsg.Class = MessageClass.RESPONSE;

			if ( resMsg.Date == null )
				resMsg.Date = DateTime.Now;
			#endregion

			#region Body
			if ( resMsg.Body != null )
			{
				if ( String.IsNullOrWhiteSpace(resMsg.Body.Type) )
					resMsg.Body.Type = MediaType.GetMimeByFileName(resMsg.Body.Name);

				if ( resMsg.Body.Length == null )
				{
					using ( MessageBody body = this.Channel.GetMessageBody(resMsg.LINK) )
					{
						resMsg.Body.Length = body.Length;
					}
				}
			}
			#endregion

			#region Contents
			foreach ( MessageContentInfo contentInfo in resMsg.Contents )
			{
				if ( String.IsNullOrWhiteSpace(contentInfo.Type) )
					contentInfo.Type = MediaType.GetMimeByFileName(contentInfo.Name);

				if ( contentInfo.Length == null )
				{
					using ( MessageContent content = this.Channel.GetMessageContent(contentInfo.LINK) )
					{
						contentInfo.Length = content.Length;
					}
				}
			}
			#endregion

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="inMsg"></param>
		/// <param name="resMsg"></param>
		protected virtual void CorrelateMessages(Message inMsg, Message resMsg)
		{
			#region Validate parameters
			if ( inMsg == null )
				throw new ArgumentNullException("inMsg");

			if ( resMsg == null )
				throw new ArgumentNullException("resMsg");
			#endregion

			if ( String.IsNullOrWhiteSpace(resMsg.To) )
				resMsg.To = inMsg.From;

			if ( inMsg.CorrLINK == null )
				inMsg.CorrLINK = resMsg.LINK;

			if ( inMsg.CorrGUID == null )
				inMsg.CorrGUID = resMsg.GUID;

			if ( resMsg.CorrLINK == null )
				resMsg.CorrLINK = inMsg.LINK;

			if ( resMsg.CorrGUID == null )
				resMsg.CorrGUID = inMsg.GUID;
		}
		#endregion


		#region Logs
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		public virtual void LogTrace(string text)
		{
			this.Channel.LogTrace(text);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		public virtual void LogInfo(string text)
		{
			this.Channel.LogInfo(text);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="error"></param>
		public virtual void LogError(Exception error)
		{
			this.Channel.LogError(error);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="error"></param>
		public virtual void LogError(string text, Exception error)
		{
			this.Channel.LogError(text, error);
		}
		#endregion

	}
}
