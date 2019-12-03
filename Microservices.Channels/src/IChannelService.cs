using System;
using System.Collections.Generic;

using Microservices.Channels.Configuration;

using Microsoft.Extensions.Hosting;

namespace Microservices.Channels
{
	public interface IChannelService : IHostedService
	{

		#region Events
		event Func<Message[], bool> SendMessages;

		event Action<string, object> StatusChanged;
		#endregion


		#region Properties
		string ProcessId { get; }

		string VirtAddress { get; }

		ChannelStatus Status { get; }
		#endregion


		#region Control
		void Open();

		void Close();

		void Run();

		void Stop();
		#endregion


		#region Diagnostic
		bool TryConnect(out Exception error);

		void CheckState();

		void Repair();

		void Ping();
		#endregion


		#region Error
		/// <summary>
		/// Сбросить ошибку.
		/// </summary>
		void ClearError();

		/// <summary>
		/// Запомнить ошибку.
		/// </summary>
		/// <param name="error"></param>
		void SetError(Exception error);

		/// <summary>
		/// Вызвать ошибку.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		ChannelException ThrowError(string text);
		#endregion


		#region Messages
		///// <summary>
		///// Опубликовать сообщение.
		///// </summary>
		///// <param name="msgLink"></param>
		//void PublishMessage(int msgLink);

		/// <summary>
		/// Принять сообщение.
		/// </summary>
		/// <param name="msgLink"></param>
		/// <returns></returns>
		int? ReceiveMessage(int msgLink);

		/// <summary>
		/// Отправить сообщение.
		/// </summary>
		/// <param name="msgLink"></param>
		/// <returns></returns>
		void SendMessage(int msgLink);
		#endregion

	}
}
