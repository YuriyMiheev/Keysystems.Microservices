using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

//using Keysystems.RemoteMessaging.Commands;
//using Keysystems.RemoteMessaging.Lib;
//using Keysystems.RemoteMessaging.Lib.Mime;

namespace Microservices.Channels
{
	/// <summary>
	/// 
	/// </summary>
	public class MessageSender : ITargetBlock<Message>, IDisposable //MarshalByRefObject, IMessageSender
	{
		private IChannelService _service;
		//private IChannelRuntime channel;
		private ActionBlock<Message> action;
		private BufferBlock<Message> buffer;
		private System.Timers.Timer timer;
		//private ReliableToken reliableToken;
		//private WaitingToken waitingToken;
		private bool _started;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="service"></param>
		/// <param name="channel"></param>
		public MessageSender(IChannelService service)
		{
			#region Validate parameters
			if (service == null)
				throw new ArgumentNullException("service");

			//if ( channel == null )
			//	throw new ArgumentNullException("channel");
			#endregion

			_service = service;
			//this.channel = channel;
			//this.reliableToken = channel.GetReliableToken(ReliableToken.CancelEvent.ChannelStop);

			this.timer = new System.Timers.Timer() { AutoReset = false, Interval = 1 };
			this.timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public Task Completion
		{
			get
			{
				if (this.action == null)
					return null;

				return this.action.Completion;
			}
		}
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parallelTasks"></param>
		/// <param name="cancelToken"></param>
		public void StartSendBufferMessages(int parallelTasks, CancellationToken cancelToken = default(CancellationToken))
		{
			#region Validate parameters
			if (parallelTasks <= 0)
				throw new ArgumentOutOfRangeException("parallelTasks", "Значение должно быть > 0.");
			#endregion

			if (_started)
				return;

			lock (this)
			{
				if (_started)
					return;

				_started = true;

				//cancelToken.Cancelled += (s, e) =>
				//	{
				//		this.reliableToken.Cancel();
				//	};
				//this.waitingToken = new WaitingToken(this.reliableToken);
				cancelToken.Register(() => _started = false);

				this.buffer = new BufferBlock<Message>(new DataflowBlockOptions() { BoundedCapacity = 1, CancellationToken = cancelToken });
				this.action = new ActionBlock<Message>(new Action<Message>(SendBufferMessage), new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = parallelTasks, CancellationToken = cancelToken });

				this.timer.Start();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="cancelToken"></param>
		/// <returns></returns>
		public Message SendMessage(Message msg, CancellationToken cancelToken = default(CancellationToken))
		{
			#region Validate parameters
			if (msg == null)
				throw new ArgumentNullException("msg");
			#endregion

			return SendMessage(_service, msg, cancelToken);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="v"></param>
		public void SendMessageAsync(Message msg, CancellationToken cancelToken = default(CancellationToken))
		{
			#region Validate parameters
			if (msg == null)
				throw new ArgumentNullException("msg");
			#endregion

			try
			{
				PrepareSendMessage(_service, msg, cancelToken);
				_service.SaveMessage(msg, cancelToken);

				MessageValidator.CheckSendMessage(_service, msg);
				msg.SetStatus(MessageStatus.NEW);
			}
			catch (Exception ex)
			{
				msg.SetStatus(MessageStatus.ERROR, ex.ToString());
				throw;
			}
			finally
			{
				_service.SaveMessage(msg, cancelToken);
			}
		}
		#endregion


		#region ITargetBlock
		DataflowMessageStatus ITargetBlock<Message>.OfferMessage(DataflowMessageHeader messageHeader, Message messageValue, ISourceBlock<Message> source, bool consumeToAccept)
		{
			return ((ITargetBlock<Message>)this.buffer).OfferMessage(messageHeader, messageValue, source, consumeToAccept);
		}

		#region IDataflowBlock
		Task IDataflowBlock.Completion
		{
			get { return this.buffer.Completion; }
		}

		void IDataflowBlock.Complete()
		{
			this.buffer.Complete();
		}

		void IDataflowBlock.Fault(Exception exception)
		{
			((IDataflowBlock)this.buffer).Fault(exception);
		}
		#endregion

		#endregion


		#region Event Handlers
		void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			var thisTimer = (System.Timers.Timer)sender;

			Message msg;
			while (this.buffer.TryReceive(out msg))
			{
				this.action.SendAsync(msg);
			}

			//if ( !this.waitingToken.CancelToken.IsCancellationRequested )
			if (_started)
				thisTimer.Start();
		}
		#endregion


		#region Static
		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="msg"></param>
		/// <param name="cancelToken"></param>
		public static void PrepareSendMessage(IChannelService channel, Message msg, CancellationToken cancelToken)
		{
			#region Validate parameters
			if (channel == null)
				throw new ArgumentNullException("channel");

			if (msg == null)
				throw new ArgumentNullException("msg");
			#endregion


			#region Header
			msg.Channel = channel.VirtAddress;

			if (msg.LINK == 0)
				msg.SetStatus(MessageStatus.NULL);

			if (String.IsNullOrWhiteSpace(msg.From))
				msg.From = channel.VirtAddress;

			if (String.IsNullOrWhiteSpace(msg.Version))
				msg.Version = MessageVersion.Current;

			if (String.IsNullOrWhiteSpace(msg.Type))
				msg.Type = MessageType.DOCUMENT;

			if (String.IsNullOrWhiteSpace(msg.Direction))
				msg.Direction = MessageDirection.OUT;

			if (String.IsNullOrWhiteSpace(msg.Class))
				msg.Class = MessageClass.REQUEST;

			if (msg.Date == null)
				msg.Date = DateTime.Now;
			#endregion

			#region Body
			if (msg.Body != null)
			{
				if (String.IsNullOrWhiteSpace(msg.Body.Type))
					msg.Body.Type = MediaType.GetMimeByFileName(msg.Body.Name);

				if (msg.Body.Length == null)
				{
					using (MessageBody body = channel.GetMessageBody(msg.LINK, reliable))
					{
						msg.Body.Length = body.Length;
					}
				}
			}
			#endregion

			#region Contents
			foreach (MessageContentInfo contentInfo in msg.Contents)
			{
				if (String.IsNullOrWhiteSpace(contentInfo.Type))
					contentInfo.Type = MediaType.GetMimeByFileName(contentInfo.Name);

				if (contentInfo.Length == null)
				{
					using (MessageContent content = channel.GetMessageContent(contentInfo.LINK, reliable))
					{
						contentInfo.Length = content.Length;
					}
				}
			}
			#endregion

		}
		#endregion


		#region Helpers
		private void SendBufferMessage(Message msg)
		{
			try
			{
				Message resMsg = SendMessage(this.channel, msg, this.reliableToken);
			}
			catch (Exception ex)
			{
				this.channel.LogTrace(ex.ToString());
			}
		}

		private Message SendMessage(IChannelService service, Message srcMsg, CancellationToken cancelToken)
		{
			IChannel destChannel;
			try
			{
				BeforeSendMessage(srcChannel, srcMsg, srcToken, out destChannel);
			}
			catch (Exception ex)
			{
				srcMsg.SetStatus(MessageStatus.ERROR, ex.ToString());
				try
				{
					srcChannel.SaveMessage(srcMsg, srcToken);
				}
				catch { }

				throw;
			}

			Message destMsg;
			Message resMsg;

			using (ReliableToken destToken = destChannel.GetReliableToken(ReliableToken.CancelEvent.ChannelClose))
			{
				//destToken.Cancelled += (s, e) => srcToken.Cancel();

				try
				{
					destMsg = DestChannelProcessMessage(srcChannel, srcMsg, srcToken, destChannel, destToken);

					if (srcMsg.Async)
						srcMsg.SetStatus(MessageStatus.DELIVERED);
					else
						srcMsg.SetStatus(MessageStatus.COMPLETED);
				}
				catch (Exception ex)
				{
					srcMsg.SetStatus(MessageStatus.ERROR, ex.ToString());
					throw;
				}
				finally
				{
					try
					{
						srcChannel.SaveMessage(srcMsg, srcToken);
					}
					catch { }
				}

				try
				{
					resMsg = ProcessSendResult(srcChannel, srcMsg, srcToken, destChannel, destMsg, destToken);
				}
				catch (Exception ex)
				{
					srcMsg.SetStatus(MessageStatus.ERROR, ex.ToString());
					try
					{
						srcChannel.SaveMessage(srcMsg, srcToken);
					}
					catch { }

					throw;
				}
			}

			return resMsg;
		}

		private void BeforeSendMessage(IChannelService srcChannel, Message srcMsg, ReliableToken srcToken, out IChannel destChannel)
		{
			PrepareSendMessage(srcChannel, srcMsg, srcToken);
			srcChannel.SaveMessage(srcMsg, srcToken);

			MessageValidator.CheckSendMessage(srcChannel.GetInfo(), srcMsg);
			destChannel = this.service.ChannelManager.GetChannel(srcMsg.To);

			srcMsg.SetStatus(MessageStatus.SENDING);
			srcChannel.SaveMessage(srcMsg, srcToken);
		}

		private Message DestChannelProcessMessage(IChannelRuntime srcChannel, Message srcMsg, ReliableToken srcToken, IChannel destChannel, ReliableToken destToken)
		{
			Message destMsg = null;
			try
			{
				destMsg = destChannel.FindMessage(srcMsg.GUID, MessageDirection.IN, destToken);
			}
			catch { }

			if (destMsg != null)
			{
				if (destMsg.Status.IsDraft)
					destChannel.DeleteMessage(destMsg.LINK, destToken);
				else
					throw new MessageException(String.Format("Входящее сообщение [{0}] уже существует ({1}) на стороне получателя {2}.", srcMsg.GUID, destMsg, destChannel));
			}

			destMsg = DestChannelCreateMessage(srcChannel, srcMsg, srcToken, destChannel, destToken);
			return DestChannelReceiveMessage(srcChannel, srcMsg, srcToken, destChannel, destMsg, destToken);
		}

		private Message DestChannelCreateMessage(IChannelRuntime srcChannel, Message srcMsg, ReliableToken srcToken, IChannel destChannel, ReliableToken destToken)
		{
			Message destMsg;

			try
			{
				#region Header
				destMsg = srcMsg.Copy();
				destMsg.Direction = MessageDirection.IN;
				destChannel.SaveMessage(destMsg, destToken);
				#endregion

				#region Body
				if (srcMsg.Body != null)
				{
					using (MessageBody srcBody = srcChannel.GetMessageBody(srcMsg.LINK, srcToken))
					{
						using (MessageBody destBody = srcBody.Copy())
						{
							destBody.MessageLINK = destMsg.LINK;
							destChannel.SaveMessageBody(destBody, destToken);
						}
					}
				}
				#endregion

				#region Contents
				foreach (MessageContentInfo contentInfo in srcMsg.Contents)
				{
					using (MessageContent srcContent = srcChannel.GetMessageContent(contentInfo.LINK, srcToken))
					{
						using (MessageContent destContent = srcContent.Copy())
						{
							destContent.MessageLINK = destMsg.LINK;
							destChannel.SaveMessageContent(destContent, destToken);
						}
					}
				}
				#endregion
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(String.Format("Ошибка передачи сообщения {0} от отправителя {1} получателю {2}.", srcMsg, srcChannel, destChannel), ex);
			}

			return destChannel.GetMessage(destMsg.LINK, destToken);
		}

		private Message DestChannelReceiveMessage(IChannelRuntime srcChannel, Message srcMsg, ReliableToken srcToken, IChannel destChannel, Message destMsg, ReliableToken destToken)
		{
			int? resLink;

			try
			{
				if (destMsg.Type == MessageType.COMMAND)
					resLink = DestChannelExecuteCommand(srcChannel, destChannel, destMsg, destToken);
				else
					resLink = destChannel.ReceiveMessage(destMsg.LINK, destToken);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(String.Format("Ошибка приема сообщения {0} получателем {1}.", destMsg, destChannel), ex);
			}

			if (resLink == null || resLink == 0)
				return null;

			return destChannel.GetMessage(resLink.Value, destToken);
		}

		private int? DestChannelExecuteCommand(IChannelRuntime srcChannel, IChannel destChannel, Message destMsg, ReliableToken destToken)
		{
			int? resLink;

			switch (destMsg.Name)
			{
				#region Стандартные команды
				case MessageCommand.GET_MESSAGE:
					{
						var command = new GetMessageCommand(this.service);
						resLink = command.Execute(destChannel, destMsg.LINK, destToken, srcChannel.VirtAddress);
					}
					break;
				case MessageCommand.GET_MESSAGE_BODY:
					{
						var command = new GetMessageBodyCommand(this.service);
						resLink = command.Execute(destChannel, destMsg.LINK, destToken, srcChannel.VirtAddress);
					}
					break;
				case MessageCommand.GET_MESSAGE_CONTENT:
					{
						var command = new GetMessageContentCommand(this.service);
						resLink = command.Execute(destChannel, destMsg.LINK, destToken, srcChannel.VirtAddress);
					}
					break;
				#endregion

				#region Команды канала
				default:
					{
						resLink = destChannel.ReceiveMessage(destMsg.LINK, destToken);
					}
					break;
					#endregion
			}

			return resLink;
		}

		private Message ProcessSendResult(IChannelRuntime srcChannel, Message srcMsg, ReliableToken srcToken, IChannel destChannel, Message destMsg, ReliableToken destToken)
		{
			if (destMsg == null)
				return null;

			Message resMsg;
			try
			{
				#region Header
				resMsg = destMsg.Copy();
				resMsg.Direction = MessageDirection.IN;
				srcChannel.SaveMessage(resMsg, srcToken);

				CorrelateMessages(srcMsg, resMsg);
				srcChannel.SaveMessage(srcMsg, srcToken);
				srcChannel.SaveMessage(resMsg, srcToken);
				#endregion

				#region Body
				if (destMsg.Body != null)
				{
					using (MessageBody destBody = destChannel.GetMessageBody(destMsg.LINK, destToken))
					{
						using (MessageBody resBody = destBody.Copy())
						{
							resBody.MessageLINK = resMsg.LINK;
							srcChannel.SaveMessageBody(resBody, srcToken);
						}
					}
				}
				#endregion

				#region Contents
				foreach (MessageContentInfo contentInfo in destMsg.Contents)
				{
					using (MessageContent destContent = destChannel.GetMessageContent(contentInfo.LINK, destToken))
					{
						using (MessageContent resContent = destContent.Copy())
						{
							resContent.MessageLINK = resMsg.LINK;
							srcChannel.SaveMessageContent(resContent, srcToken);
						}
					}
				}
				#endregion
			}
			catch (Exception ex)
			{
				var error = new InvalidOperationException(String.Format("Ошибка обработки ответного сообщения {0}.", destMsg), ex);
				destMsg.SetStatus(MessageStatus.ERROR, error.ToString());
				try
				{
					destChannel.SaveMessage(destMsg, destToken);
				}
				catch { }

				throw;
			}


			try
			{
				int? answLink = srcChannel.ReceiveMessage(resMsg.LINK, srcToken);

				if (destMsg.Async)
					destMsg.SetStatus(MessageStatus.DELIVERED);
				else
					destMsg.SetStatus(MessageStatus.COMPLETED);
			}
			catch (Exception ex)
			{
				var error = new InvalidOperationException(String.Format("Ошибка на стороне получателя {0}.", srcChannel), ex);
				destMsg.SetStatus(MessageStatus.ERROR, error.ToString());
			}
			finally
			{
				try
				{
					destChannel.SaveMessage(destMsg, destToken);
				}
				catch { }
			}

			return srcChannel.GetMessage(resMsg.LINK);
		}

		private void CorrelateMessages(Message srcMsg, Message resMsg)
		{
			#region Validate parameters
			if (srcMsg == null)
				throw new ArgumentNullException("srcMsg");

			if (resMsg == null)
				throw new ArgumentNullException("resMsg");
			#endregion

			if (srcMsg.CorrLINK == null)
				srcMsg.CorrLINK = resMsg.LINK;

			if (srcMsg.CorrGUID == null)
				srcMsg.CorrGUID = resMsg.GUID;

			if (resMsg.CorrLINK == null)
				resMsg.CorrLINK = srcMsg.LINK;

			if (resMsg.CorrGUID == null)
				resMsg.CorrGUID = srcMsg.GUID;
		}
		#endregion


		#region MarshalByRefObject
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override object InitializeLifetimeService()
		{
			return null;
		}
		#endregion


		#region IDisposable
		private bool disposed = false;

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				this.timer.Dispose();
				this.reliableToken.Dispose();
				if (this.waitingToken != null)
					this.waitingToken.Dispose();

				this.service = null;
				this.channel = null;
			}

			disposed = true;
		}

		/// <summary>
		/// Деструктор.
		/// </summary>
		~MessageSender()
		{
			Dispose(false);
		}
		#endregion

	}
}
