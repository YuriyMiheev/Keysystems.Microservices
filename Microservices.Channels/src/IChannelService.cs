using System;

using Microsoft.Extensions.Hosting;

namespace Microservices.Channels
{
	public interface IChannelService : IHostedService//, IDisposable
	{

		#region Properties
		int ProcessId { get; }

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
