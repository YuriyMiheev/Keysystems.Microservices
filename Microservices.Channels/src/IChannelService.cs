using Microsoft.Extensions.Hosting;

namespace Microservices.Channels
{
	public interface IChannelService : IHostedService
	{

		#region Properties
		/// <summary>
		/// ИД процесса.
		/// </summary>
		int ProcessId { get; }

		/// <summary>
		/// ИД канала.
		/// </summary>
		int LINK { get; }

		/// <summary>
		/// Вирт.адрес канала.
		/// </summary>
		string VirtAddress { get; }
		#endregion



		//#region Messages
		/////// <summary>
		/////// Опубликовать сообщение.
		/////// </summary>
		/////// <param name="msgLink"></param>
		////void PublishMessage(int msgLink);

		///// <summary>
		///// Принять сообщение.
		///// </summary>
		///// <param name="msgLink"></param>
		///// <returns></returns>
		//int? ReceiveMessage(int msgLink);

		///// <summary>
		///// Отправить сообщение.
		///// </summary>
		///// <param name="msgLink"></param>
		///// <returns></returns>
		//void SendMessage(int msgLink);
		//#endregion

	}
}
