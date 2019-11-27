using System;
using System.Collections.Generic;

using Microservices.Channels.Adapters;
using Microservices.Channels.Configuration;
using Microservices.Channels.Data;
using Microservices.Channels.Logging;

using Microsoft.Extensions.Hosting;

namespace Microservices.Channels
{
	public interface IChannelService : IHostedService, IMessageRepository
	{

		MessageDataAdapterBase MessageDataAdapter { get; }


		#region Events
		event Func<Message[], bool> OutMessages;
		#endregion


		#region Properties
		string ProcessId { get; }

		string VirtAddress { get; }

		bool Opened { get; }

		bool Running { get; }

		bool? Online { get; }
		#endregion


		#region Settings
		InfoSettings InfoSettings { get; }

		ChannelSettings ChannelSettings { get; }

		DatabaseSettings DatabaseSettings { get; }

		MessageSettings MessageSettings { get; }

		ServiceSettings ServiceSettings { get; }

		IDictionary<string, ConfigFileSetting> GetAppSettings();

		void SetAppSettings(IDictionary<string, string> settings);

		void SaveAppSettings();
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
