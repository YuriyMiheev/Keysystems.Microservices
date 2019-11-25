using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Channels.Configuration;
using Microservices.Channels.Data;
using Microservices.Channels.Logging;

namespace Microservices.Channels
{
	public interface IChannelService : IMessageRepository, ILogger
	{

		#region Properties
		string VirtAddress { get; }

		bool Opened { get; }

		bool Running { get; }

		bool? Online { get; }
        #endregion

        #region Settings
        #endregion


        #region Settings
        ChannelConfigFileSettings InfoSettings { get; }

        ServiceConfigFileSettings ServiceSettings { get; }

        DatabaseSettings DatabaseSettings { get; }

		MessageSettings MessageSettings { get; }
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
		//IAsyncEnumerable<Message> ScanSendMessages(CancellationToken cancellationToken = default(CancellationToken));

		//IAsyncEnumerable<Message> ScanPublishMessages(CancellationToken cancellationToken = default(CancellationToken));

		///// <summary>
		///// Отправить сообщение.
		///// </summary>
		///// <param name="msgLink"></param>
		///// <returns></returns>
		//int? SendMessage(int msgLink);

		///// <summary>
		///// Отправить сообщение асинхронно.
		///// </summary>
		///// <param name="msgLink"></param>
		//void SendMessageAsync(int msgLink);

		//Message PreSendMessage(int msgLink);

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
		#endregion

	}
}
