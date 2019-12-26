using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Channels;
using Microservices.Configuration;
using Microservices.Data;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Клиент (коннектор) канала.
	/// </summary>
	public interface IChannelClient : IDisposable
	{

		#region Events
		/// <summary>
		/// Подключение установлено.
		/// </summary>
		event Action<IChannelClient> Connected;

		/// <summary>
		/// Переподключение началось.
		/// </summary>
		event Func<IChannelClient, Exception, Task> Reconnecting;

		/// <summary>
		/// Переподключение произошло.
		/// </summary>
		event Func<IChannelClient, string, Task> Reconnected;

		/// <summary>
		/// Подключение разорвано.
		/// </summary>
		event Func<IChannelClient, Exception, Task> Disconnected;
		
		/// <summary>
		/// Лог принят.
		/// </summary>
		event Action<IChannelClient, IDictionary<string, object>> LogReceived;

		/// <summary>
		/// Сообщения приняты.
		/// </summary>
		event Action<IChannelClient, Message[]> MessagesReceived;
		#endregion


		#region Properties
		/// <summary>
		/// Статус канала.
		/// </summary>
		ChannelStatus Status { get; }
		
		/// <summary>
		/// Признак подключения к каналу.
		/// </summary>
		bool IsConnected { get; }
		#endregion


		#region Connect/Disconnect
		Task ConnectAsync(string accessKey, CancellationToken cancellationToken = default);

		Task DisconnectAsync(CancellationToken cancellationToken = default);
		#endregion


		#region Control
		Task OpenChannelAsync(CancellationToken cancellationToken = default);

		Task CloseChannelAsync(CancellationToken cancellationToken = default);

		Task RunChannelAsync(CancellationToken cancellationToken = default);

		Task StopChannelAsync(CancellationToken cancellationToken = default);
		#endregion


		#region Diagnostic
		Task<Exception> TryConnectToChannelAsync(CancellationToken cancellationToken = default);

		Task<Exception> CheckChannelStateAsync(CancellationToken cancellationToken = default);

		Task RepairChannelAsync(CancellationToken cancellationToken = default);

		Task PingChannelAsync(CancellationToken cancellationToken = default);
		#endregion


		#region Settings
		Task<IDictionary<string, AppConfigSetting>> GetChannelSettingsAsync(CancellationToken cancellationToken = default);

		Task SetChannelSettingsAsync(IDictionary<string, string> settings, CancellationToken cancellationToken = default);

		Task SaveChannelSettings(CancellationToken cancellationToken = default);
		#endregion


		#region Messages
		Task<List<Message>> SelectMessagesAsync(QueryParams queryParams, CancellationToken cancellationToken = default);

		Task<(List<Message>, int)> GetMessagesAsync(string status, int? skip, int? take, CancellationToken cancellationToken = default);

		Task<(List<Message>, int)> GetLastMessagesAsync(string status, int? skip, int? take, CancellationToken cancellationToken = default);

		Task<Message> GetMessageAsync(int msgLink, CancellationToken cancellationToken = default);

		//Task<Message> FindMessageAsync(int msgLink, CancellationToken cancellationToken = default);

		Task<Message> FindMessageByGuidAsync(string msgGuid, string direction, CancellationToken cancellationToken = default);

		Task SaveMessageAsync(Message msg, CancellationToken cancellationToken = default);

		Task DeleteMessageAsync(int msgLink, CancellationToken cancellationToken = default);

		//Task DeleteExpiredMessagesAsync(DateTime expiredDate, List<string> statuses, CancellationToken cancellationToken = default);

		Task DeleteMessagesAsync(IEnumerable<int> msgLinks, CancellationToken cancellationToken = default);

		Task<int?> ReceiveMessageAsync(int msgLink, CancellationToken cancellationToken = default);


		#region Body
		Task<TextReader> ReadMessageBodyAsync(int msgLink, CancellationToken cancellationToken = default);

		Task SaveMessageBodyAsync(MessageBodyInfo bodyInfo, TextReader bodyStream, CancellationToken cancellationToken = default);
		#endregion


		#region Content
		Task<TextReader> ReadMessageContentAsync(int contentLink, CancellationToken cancellationToken = default);

		Task SaveMessageContentAsync(MessageContentInfo contentInfo, TextReader contentStream, CancellationToken cancellationToken = default);
		#endregion

		#endregion

	}
}
