using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Configuration;
using Microservices.Data;

namespace Microservices.Channels.Hubs
{
	public interface IChannelHub
	{

		IDictionary<string, object> Login(string accessKey);


		#region Control
		void OpenChannel();

		void CloseChannel();

		void RunChannel();

		void StopChannel();
		#endregion


		#region Diagnostic
		Exception TryConnect();

		Exception CheckState();

		void Repair();

		void Ping();
		#endregion


		#region Settings
		IDictionary<string, AppConfigSetting> GetSettings();

		void SetSettings(IDictionary<string, string> settings);

		void SaveSettings();
		#endregion


		#region Messages
		List<Message> SelectMessages(QueryParams queryParams);

		(List<Message>, int) GetMessages(string status, int? skip, int? take);

		(List<Message>, int) GetLastMessages(string status, int? skip, int? take);

		Message GetMessage(int msgLink);

		//Message FindMessage(int msgLink);

		Message FindMessageByGuid(string msgGuid, string direction);

		void SaveMessage(Message msg);

		void DeleteMessage(int msgLink);

		//void DeleteExpiredMessages(DateTime expiredDate, List<string> statuses);

		void DeleteMessages(IEnumerable<int> msgLinks);


		#region Body
		/// <summary>
		/// Получить тело сообщения.
		/// </summary>
		/// <param name="msgLink"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		IAsyncEnumerable<char[]> ReadMessageBody(int msgLink, CancellationToken cancellationToken = default);

		/// <summary>
		/// Сохранить тело сообщения.
		/// </summary>
		/// <param name="bodyInfo"></param>
		/// <param name="bodyStream"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task SaveMessageBody(MessageBodyInfo bodyInfo, IAsyncEnumerable<char[]> bodyStream, CancellationToken cancellationToken = default);

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
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		IAsyncEnumerable<char[]> ReadMessageContent(int contentLink, CancellationToken cancellationToken = default);

		/// <summary>
		/// Сохранить контент сообщения.
		/// </summary>
		/// <param name="contentInfo"></param>
		/// <param name="contentStream"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task SaveMessageContent(MessageContentInfo contentInfo, IAsyncEnumerable<char[]> contentStream, CancellationToken cancellationToken = default);

		/// <summary>
		/// Удалить контент сообщения.
		/// </summary>
		/// <param name="contentLink"></param>
		void DeleteMessageContent(int contentLink);
		#endregion


		int? ReceiveMessage(int msgLink);

		void SendMessage(int msgLink);
		#endregion

	}
}
