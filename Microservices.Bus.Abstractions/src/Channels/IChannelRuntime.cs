using System;
using System.Collections.Generic;

using Microservices.Data;
using Microservices.Data.DAO;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Исполняемый модуль канала.
	/// </summary>
	public interface IChannelRuntime : IMessageRepository, IDisposable
	{

		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		string Id { get; }

		/// <summary>
		/// {Get}
		/// </summary>
		int LINK { get; }

		/// <summary>
		/// {Get}
		/// </summary>
		string Provider { get; }

		/// <summary>
		/// {Get}
		/// </summary>
		string VirtAddress { get; }

		///// <summary>
		///// {Get}
		///// </summary>
		//ChannelSettings ChannelSettings { get; }

		///// <summary>
		///// {Get}
		///// </summary>
		//MessageSettings MessageSettings { get; }

		/// <summary>
		/// {Get} Канал открыт.
		/// </summary>
		bool IsOpened { get; }

		/// <summary>
		/// {Get} Канал разрушен.
		/// </summary>
		bool IsDisposed { get; }
		#endregion


		#region Methods

		#region Info
		/// <summary>
		/// Получить информацию о канале.
		/// </summary>
		/// <returns></returns>
		ChannelInfo GetInfo();
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
		void SetError(ExceptionWrapper error);

		/// <summary>
		/// Вызвать ошибку.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		Exception ThrowError(string text);
		#endregion


		#region Control
		/// <summary>
		/// Открыть канал.
		/// </summary>
		/// <exception cref="Keysystems.RemoteMessaging.Services.Channels.ChannelException"></exception>
		void Open();

		/// <summary>
		/// Закрыть канал.
		/// </summary>
		void Close();

		/// <summary>
		/// Запустить канал.
		/// </summary>
		void Run();

		/// <summary>
		/// Остановить канал.
		/// </summary>
		void Stop();
		#endregion


		#region Diagnostic
		/// <summary>
		/// Проверить подключение к каналу.
		/// </summary>
		/// <param name="error"></param>
		/// <returns></returns>
		bool TryConnect(out Exception error);

		/// <summary>
		/// Проверить состояние канала.
		/// </summary>
		void CheckState();

		/// <summary>
		/// Попытаться восстановить работоспособность канала.
		/// </summary>
		void Repair();

		/// <summary>
		/// Ping канала.
		/// </summary>
		void Ping();
		#endregion


		#region Export/Import
		///// <summary>
		///// Импортировать сообщение из контейнера.
		///// </summary>
		///// <param name="container"></param>
		///// <returns></returns>
		//int ImportMessage(IMessageReader container);

		///// <summary>
		///// Экспортировать сообщение в контейнер.
		///// </summary>
		///// <param name="msgLink"></param>
		///// <param name="container"></param>
		//void ExportMessage(int msgLink, IMessageWriter container);
		#endregion


		#region Messages
		/// <summary>
		/// Отправить сообщение.
		/// </summary>
		/// <param name="msgLink"></param>
		/// <returns></returns>
		int? SendMessage(int msgLink);

		/// <summary>
		/// Отправить сообщение асинхронно.
		/// </summary>
		/// <param name="msgLink"></param>
		void SendMessageAsync(int msgLink);

		/// <summary>
		/// Опубликовать сообщение.
		/// </summary>
		/// <param name="msgLink"></param>
		void PublishMessage(int msgLink);

		/// <summary>
		/// Принять сообщение.
		/// </summary>
		/// <param name="msgLink"></param>
		/// <returns></returns>
		int? ReceiveMessage(int msgLink);
		#endregion


		#region Contacts
		/// <summary>
		/// Получить список контактов.
		/// </summary>
		/// <returns></returns>
		List<Contact> GetContacts();

		/// <summary>
		/// Синхронизироваться со списком контактов.
		/// </summary>
		void SyncContacts();
		#endregion

		#endregion

	}
}
