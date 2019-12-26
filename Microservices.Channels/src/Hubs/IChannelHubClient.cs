using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservices.Channels.Hubs
{
	/// <summary>
	/// Клиент хаба.
	/// </summary>
	public interface IChannelHubClient
	{
		/// <summary>
		/// Отправка  сообщений клиенту.
		/// </summary>
		/// <param name="messages"></param>
		/// <returns></returns>
		Task ReceiveMessages(Message[] messages);

		/// <summary>
		/// Отправка статуса клиенту.
		/// </summary>
		/// <param name="statusName"></param>
		/// <param name="statusValue"></param>
		/// <returns></returns>
		Task ReceiveStatus(string statusName, object statusValue);
		
		/// <summary>
		/// Отправка лога клиенту.
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		Task ReceiveLog(IDictionary<string, object> record);

	}
}
