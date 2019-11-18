using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

//using Keysystems.RemoteMessaging.Containers;
//using Keysystems.RemoteMessaging.Services.Channels;
//using Keysystems.RemoteMessaging.Lib.Mime;

namespace Microservices.Channels
{
	/// <summary>
	/// 
	/// </summary>
	public static class MessageValidator
	{

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="virtAddress"></param>
		public static bool IsAddressValid(string virtAddress)
		{
			if ( String.IsNullOrWhiteSpace(virtAddress) )
				return false;

			try
			{
				var address = new MailAddress(virtAddress);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="msg"></param>
		public static void CheckSendMessage(this IChannelService channel, Message msg)
		{
			#region Validate parameters
			if ( channel == null )
				throw new ArgumentNullException("channel");

			if ( msg == null )
				throw new ArgumentNullException("msg");
			#endregion

			if ( String.IsNullOrEmpty(msg.GUID) )
				throw new MessageException("В сообщении отсутствует GUID.");

			if ( !msg.Channel.Equals(channel.VirtAddress, StringComparison.InvariantCultureIgnoreCase) )
				throw new MessageException("Сообщение не принадлежит каналу отправителю.");

			if ( (msg.Class != MessageClass.REQUEST) && (msg.Class != MessageClass.RESPONSE) )
				throw new MessageException(String.Format("Недопустимый класс '{0}' сообщения.", msg.Class));

			if ( (msg.Type != MessageType.DOCUMENT) && (msg.Type != MessageType.COMMAND) && (msg.Type != MessageType.EVENT) )
				throw new MessageException(String.Format("Неизвестный тип '{0}' сообщения.", msg.Type));

			if ( msg.Direction != MessageDirection.OUT )
				throw new MessageException("Сообщение не является исходящим.");

			if ( !msg.Status.IsDraft && !msg.Status.IsNew )
				throw new MessageException(String.Format("Сообщение имеет недопустимый статус '{0}'.", msg.Status.Value));

			List<string> recipients = msg.GetRecipients();
			if ( recipients.Count == 0 )
				throw new MessageException("Не указан получатель сообщения.");

			if ( recipients.Count > 1 )
				throw new MessageException("Разрешен только один получатель сообщения.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="msg"></param>
		public static void CheckPublishMessage(this IChannelService channel, Message msg)
		{
			#region Validate parameters
			if ( channel == null )
				throw new ArgumentNullException("channel");

			if ( msg == null )
				throw new ArgumentNullException("msg");
			#endregion

			if ( String.IsNullOrEmpty(msg.GUID) )
				throw new MessageException("В сообщении отсутствует GUID.");

			if ( !msg.Channel.Equals(channel.VirtAddress, StringComparison.InvariantCultureIgnoreCase) )
				throw new MessageException("Сообщение не принадлежит каналу издателю.");

			if ( String.IsNullOrWhiteSpace(msg.Class) )
				throw new MessageException("В сообщении отсутствует информация о классе сообщения.");

			if ( msg.Class != MessageClass.PUBLISH )
				throw new MessageException("Сообщение не является публикуемым.");

			if ( (msg.Type != MessageType.DOCUMENT) && (msg.Type != MessageType.COMMAND) && (msg.Type != MessageType.EVENT) )
				throw new MessageException(String.Format("Неизвестный тип '{0}' сообщения.", msg.Type));

			if ( msg.Direction != MessageDirection.OUT )
				throw new MessageException("Сообщение не является исходящим.");

			if ( !msg.Status.IsDraft && !msg.Status.IsNew )
				throw new MessageException(String.Format("Некорректный статус '{0}' публикуемого сообщения {1}.", msg.Status, msg));

			if ( msg.GetRecipients().Count == 0 )
				throw new MessageException("Не указаны подписчики сообщения.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="msg"></param>
		public static void CheckResponseMessage(this IChannelService channel, Message msg)
		{
			#region Validate parameters
			if ( channel == null )
				throw new ArgumentNullException("channel");

			if ( msg == null )
				throw new ArgumentNullException("msg");
			#endregion

			if ( String.IsNullOrEmpty(msg.GUID) )
				throw new MessageException("В сообщении отсутствует GUID.");

			if ( msg.Direction != MessageDirection.OUT )
				throw new MessageException("Cообщение не является исходящим.");

			if ( msg.Class != MessageClass.RESPONSE )
				throw new MessageException(String.Format("Сообщение {0} не является ответным.", msg));

			if ( String.IsNullOrWhiteSpace(msg.Type) )
				throw new MessageException("В сообщении отсутствует информация о типе сообщения.");

			if ( !msg.Channel.Equals(channel.VirtAddress, StringComparison.InvariantCultureIgnoreCase) )
				throw new MessageException("Сообщение не принадлежит каналу отправителю.");

			List<string> recipients = msg.GetRecipients();
			if ( recipients.Count == 0 )
				throw new MessageException("Не указан получатель(и) сообщения.");

			if ( !msg.Status.IsDraft && !msg.Status.IsNew )
				throw new MessageException(String.Format("Некорректный статус '{0}' ответного сообщения {1}.", msg.Status, msg));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="msg"></param>
		public static void CheckImportMessage(this IChannelService channel, Message msg)
		{
			#region Validate parameters
			if ( msg == null )
				throw new ArgumentNullException("msg");
			#endregion

			if ( msg.LINK != 0 )
				throw new MessageException("LINK сообщения должнен быть равен 0.");

			if ( String.IsNullOrEmpty(msg.GUID) )
				throw new MessageException("В сообщении отсутствует GUID.");

			if ( String.IsNullOrWhiteSpace(msg.Direction) )
				throw new MessageException("В сообщении не указано направление передачи.");

			if ( !String.IsNullOrWhiteSpace(msg.Type) && (msg.Type != MessageType.DOCUMENT) && (msg.Type != MessageType.COMMAND) && (msg.Type != MessageType.EVENT) )
				throw new MessageException(String.Format("Неизвестный тип '{0}' сообщения.", msg.Type));

			if ( !String.IsNullOrWhiteSpace(msg.Class) && (msg.Class != MessageClass.REQUEST) && (msg.Class != MessageClass.RESPONSE) && (msg.Class != MessageClass.PUBLISH) )
				throw new MessageException(String.Format("Неизвестный класс '{0}' сообщения.", msg.Class));

			if ( !msg.Status.IsDraft )
				throw new MessageException(String.Format("Некорректный статус '{0}' импортируемого сообщения {1}.", msg.Status, msg));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="msg"></param>
		public static void CheckReceiveMessage(this IChannelService channel, Message msg)
		{
			#region Validate parameters
			if ( channel == null )
				throw new ArgumentNullException("channel");

			if ( msg == null )
				throw new ArgumentNullException("msg");
			#endregion

			if ( String.IsNullOrEmpty(msg.GUID) )
				throw new MessageException($"В сообщении {msg} отсутствует GUID.");

			if ( msg.Direction != MessageDirection.IN )
				throw new MessageException($"Cообщение {msg} не является входящим.");

			if ( !msg.Status.IsDraft && !msg.Status.IsNew )
				throw new MessageException($"Некорректный статус '{msg.Status}' принимаемого сообщения {msg}.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="msg"></param>
		public static void CheckCommandMessage(this IChannelService channel, Message msg)
		{
			#region Validate parameters
			if ( channel == null )
				throw new ArgumentNullException("channel");

			if ( msg == null )
				throw new ArgumentNullException("msg");
			#endregion

			if ( msg.Type != MessageType.COMMAND )
				throw new MessageException(String.Format("Недопуститмый тип '{0}' сообщения.", msg.Type));
		}
		#endregion

	}
}
