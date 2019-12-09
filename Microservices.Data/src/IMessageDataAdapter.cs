using System;
using System.Collections.Generic;
using System.Data;

namespace Microservices.Data
{
	/// <summary>
	/// Адаптер хранилища сообщений.
	/// </summary>
	public interface IMessageDataAdapter
	{

		#region Properties
		DbContext DbContext { get; }

		int ExecuteTimeout { get; set; }
		#endregion


		#region Methods
		bool CheckConnection(out ConnectionException error);

		IDataQuery OpenQuery();

		IDataQuery OpenQuery(IsolationLevel transaction);

		UnitOfWork BeginWork();


		#region Messages
		/// <summary>
		/// Выбрать сообщения.
		/// </summary>
		/// <param name="queryParams"></param>
		/// <returns></returns>
		List<Message> SelectMessages(QueryParams queryParams);

		/// <summary>
		/// Получить список сообщений.
		/// </summary>
		/// <param name="status"></param>
		/// <param name="skip"></param>
		/// <param name="take"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		List<Message> GetMessages(string status, int? skip, int? take, out int totalCount);

		/// <summary>
		/// Получить список последних сообщений.
		/// </summary>
		/// <param name="status"></param>
		/// <param name="skip"></param>
		/// <param name="take"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		List<Message> GetLastMessages(string status, int? skip, int? take, out int totalCount);

		/// <summary>
		/// Получить сообщение.
		/// </summary>
		/// <param name="msgLink"></param>
		/// <returns></returns>
		Message GetMessage(int msgLink);

		///// <summary>
		///// Найти сообщение.
		///// </summary>
		///// <param name="msgLink"></param>
		///// <returns></returns>
		//Message FindMessage(int msgLink);

		/// <summary>
		/// Найти сообщение.
		/// </summary>
		/// <param name="msgGuid"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		Message FindMessage(string msgGuid, string direction);

		/// <summary>
		/// Сохранить сообщение.
		/// </summary>
		/// <param name="msg"></param>
		void SaveMessage(Message msg);

		/// <summary>
		/// Удалить сообщение.
		/// </summary>
		/// <param name="msgLink"></param>
		void DeleteMessage(int msgLink);

		///// <summary>
		///// Удалить устаревшие сообщения.
		///// </summary>
		///// <param name="expiredDate"></param>
		///// <param name="statuses"></param>
		//void DeleteExpiredMessages(DateTime expiredDate, List<string> statuses);

		/// <summary>
		/// Удалить сообщения.
		/// </summary>
		/// <param name="msgLinks"></param>
		void DeleteMessages(IEnumerable<int> msgLinks);


		#region Body
		/// <summary>
		/// Получить тело сообщения.
		/// </summary>
		/// <param name="msgLink"></param>
		/// <returns></returns>
		MessageBody GetMessageBody(int msgLink);

		/// <summary>
		/// Сохранить тело сообщения.
		/// </summary>
		/// <param name="body"></param>
		void SaveMessageBody(MessageBody body);

		/// <summary>
		/// Удалить тело сообщения.
		/// </summary>
		/// <param name="msgLink"></param>
		void DeleteMessageBody(int msgLink);
		#endregion


		#region Content
		/// <summary>
		/// Получить контент сообщения.
		/// </summary>
		/// <param name="contentLink"></param>
		/// <returns></returns>
		MessageContent GetMessageContent(int contentLink);

		/// <summary>
		/// Сохранить контент сообщения.
		/// </summary>
		/// <param name="content"></param>
		void SaveMessageContent(MessageContent content);

		/// <summary>
		/// Удалить контент сообщения.
		/// </summary>
		/// <param name="contentLink"></param>
		void DeleteMessageContent(int contentLink);
		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <param name="begin"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		List<DAO.DateStatMessage> GetMessagesByDate(DateTime? begin, DateTime? end);
		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		int ExecuteUpdate(string sql);
		#endregion

	}
}
