using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

using Microservices.Channels.Adapters;
using Microservices.Channels.Data;

using NHibernate.Criterion;

namespace Microservices.Channels.MSSQL
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class MessageScannerBase : IDisposable
	{
		private IChannelService _channelService;
		private MessageDataAdapterBase _dataAdapter;
		private System.Threading.CancellationToken _cancellationToken;
		private Timer _queryTimer;
		private bool _started;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="channelService"></param>
		/// <param name="recipient"></param>
		protected MessageScannerBase(IChannelService channelService, string recipient)
		{
			_channelService = channelService ?? throw new ArgumentNullException(nameof(channelService));
			this.Recipient = recipient ?? throw new ArgumentException("Пустой адрес получателя сообщений.", nameof(recipient));

			_queryTimer = new Timer() { AutoReset = false };
			_queryTimer.Elapsed += new ElapsedEventHandler(queryTimer_Elapsed);
		}
		#endregion


		#region Events
		public event Func<Message[], bool> NewMessages;
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public string Recipient { get; private set; }
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="channelService"></param>
		/// <param name="cancellationToken"></param>
		public virtual void Start(System.Threading.CancellationToken cancellationToken = default)
		{
			if (_started)
				return;

			_started = true;
			_dataAdapter = _channelService.MessageDataAdapter;

			_cancellationToken = cancellationToken;
			_cancellationToken.Register(() =>
				 {
					 OnCancel();
				 });

			_queryTimer.Interval = _channelService.MessageSettings.ScanInterval.TotalMilliseconds;
			_queryTimer.Start();
		}
		#endregion


		#region Event Handlers
		void queryTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (_started)
			{
				try
				{
					using IDataQuery dataQuery = _dataAdapter.OpenQuery();
					var query = CreateOfflineSelectMessagesQuery();
					List<Message> messages = query.GetExecutableQueryOver(dataQuery.Session)
						 .Take(_channelService.MessageSettings.ScanPortion).List()
						 .Select(msg => msg.ToObj()).ToList();

					LogTrace($"Найдено {messages.Count} новых сообщений.");

					if (messages.Count > 0)
					{
						PreProcessSelectedMessages(messages, _cancellationToken);

						if (this.NewMessages != null)
						{
							if (this.NewMessages.Invoke(messages.ToArray()))
							{
								string statusInfo = "Сообщение отправлено.";
								//string sql = $"UPDATE {Database.Tables.MESSAGES} SET STATUS='{MessageStatus.SENDING}', STATUS_INFO='{statusInfo}', STATUS_DATE='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}' WHERE LINK={msg.LINK}";
								//int count = _dataAdapter.ExecuteUpdate(sql);
							}
						}
					}
				}
				catch (Exception ex)
				{
					var error = new InvalidOperationException("Ошибка сканирования новых сообщений.", ex);
					SetError(ex);
					LogError(ex);
				}
				finally
				{
					if (_started)
						_queryTimer.Start();
				}
			}
		}
		#endregion


		#region Override
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected abstract QueryOver<DAO.Message, DAO.Message> CreateOfflineSelectMessagesQuery();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="messages"></param>
		/// <param name="cancellationToken"></param>
		protected virtual void PreProcessSelectedMessages(List<Message> messages, System.Threading.CancellationToken cancellationToken)
		{
			//foreach (Message msg in messages.ToList())
			//{
			//	if (msg.TTL != null && msg.TTL >= DateTime.Now)
			//	{
			//		messages.Remove(msg);

			//		try
			//		{
			//			msg.SetStatus(MessageStatus.DELETED, "Сообщение устарело.");
			//			this.dataAdapter.SaveMessage(msg);
			//		}
			//		catch { }
			//	}
			//}
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnCancel()
		{
			_queryTimer.Stop();
			_started = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="error"></param>
		protected virtual void LogError(Exception error)
		{
			_channelService?.LogError(error);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		protected virtual void LogTrace(string text)
		{
			_channelService?.LogTrace(text);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="error"></param>
		protected virtual void SetError(Exception error)
		{
			_channelService?.SetError(error);
		}
		#endregion


		#region IDisposable
		private bool disposed = false;

		/// <summary>
		/// Разрушить объект и освободить занятые им ресурсы.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Разрушить объект и освободить занятые им ресурсы.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				_queryTimer.Dispose();
				_channelService = null;
			}

			disposed = true;
		}

		/// <summary>
		/// Деструктор.
		/// </summary>
		~MessageScannerBase()
		{
			Dispose(false);
		}
		#endregion

	}
}
