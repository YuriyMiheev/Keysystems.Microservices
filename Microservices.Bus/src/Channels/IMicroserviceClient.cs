using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Channels;
using Microservices.Configuration;
using Microservices.Data;

namespace Microservices.Bus.Channels
{
	public interface IMicroserviceClient : IDisposable
	{

		#region Events
		/// <summary>
		/// Подключение установлено.
		/// </summary>
		event Action<IMicroserviceClient> Connected;

		event Func<IMicroserviceClient, Exception, Task> Reconnecting;

		event Func<IMicroserviceClient, string, Task> Reconnected;

		event Func<IMicroserviceClient, Exception, Task> Disconnected;
		
		event Action<IMicroserviceClient, IDictionary<string, object>> LogReceived;

		event Action<IMicroserviceClient, Message[]> MessagesReceived;
		#endregion


		#region Properties
		string Url { get; set; }

		IDictionary<string, object> Info { get; }

		ChannelStatus Status { get; set; }
		
		IWebProxy WebProxy { get; set; }

		bool IsConnected { get; }
		#endregion


		#region Login/Logout
		Task LoginAsync(string accessKey, CancellationToken cancellationToken = default);

		Task LogoutAsync(CancellationToken cancellationToken = default);
		#endregion


		#region Control
		Task OpenChannelAsync(CancellationToken cancellationToken = default);

		Task CloseChannelAsync(CancellationToken cancellationToken = default);

		Task RunChannelAsync(CancellationToken cancellationToken = default);

		Task StopChannelAsync(CancellationToken cancellationToken = default);
		#endregion


		#region Diagnostic
		Task<Exception> TryConnectAsync(CancellationToken cancellationToken = default);

		Task<Exception> CheckStateAsync(CancellationToken cancellationToken = default);

		Task RepairAsync(CancellationToken cancellationToken = default);

		Task PingAsync(CancellationToken cancellationToken = default);
		#endregion


		#region Settings
		Task<IDictionary<string, AppConfigSetting>> GetSettingsAsync(CancellationToken cancellationToken = default);

		Task SetSettingsAsync(IDictionary<string, string> settings, CancellationToken cancellationToken = default);

		Task SaveSettings(CancellationToken cancellationToken = default);
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
