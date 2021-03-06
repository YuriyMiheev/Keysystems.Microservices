﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

using Microservices;
using Microservices.Channels.Data;
using Microservices.Channels.Logging;
using Microservices.Data;

using NHibernate.Criterion;

using DAO = Microservices.Data.DAO;

namespace Microservices.Channels
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class DatabaseMessageScanner : IMessageScanner, IDisposable
	{
		//private MessageSettings _settings;
		private TimeSpan _interval;
		private int _portion;
		private IChannelDataAdapter _dataAdapter;
		private ILogger _logger;
		private System.Threading.CancellationToken _cancellationToken;
		private Timer _queryTimer;
		private bool _started;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataAdapter"></param>
		/// <param name="logger"></param>
		protected DatabaseMessageScanner(IChannelDataAdapter dataAdapter, ILogger logger)
		{
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			_queryTimer = new Timer() { AutoReset = false };
			_queryTimer.Elapsed += new ElapsedEventHandler(queryTimer_Elapsed);
		}
		#endregion


		#region Events
		/// <summary>
		/// 
		/// </summary>
		public event Func<Message[], bool> NewMessages;
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="interval"></param>
		/// <param name="portion"></param>
		/// <param name="cancellationToken"></param>
		public virtual void StartScan(TimeSpan interval, int portion, System.Threading.CancellationToken cancellationToken = default)
		{
			if (_started)
				return;

			_started = true;
			//this.Recipient = recipient ?? throw new ArgumentException("Пустой адрес получателя сообщений.", nameof(recipient));
			_interval = interval;
			_portion = portion;

			_cancellationToken = cancellationToken;
			_cancellationToken.Register(() =>
				 {
					 OnCancel();
				 });

			_queryTimer.Interval = interval.TotalMilliseconds;
			_queryTimer.Start();
		}
		#endregion


		#region Timer
		void queryTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (_started)
			{
				var messages = new List<Message>();
				bool sending = false;

				try
				{
					using IDataQuery dataQuery = _dataAdapter.OpenQuery();
					var query = CreateOfflineSelectMessagesQuery();
					messages = query.GetExecutableQueryOver(dataQuery.Session)
						 .Take(_portion).List()
						 .Select(msg => msg.ToObj()).ToList();

					_logger.LogTrace($"Найдено {messages.Count} новых сообщений.");

					if (messages.Count > 0)
					{
						PreProcessSelectedMessages(messages, _cancellationToken);

						if (this.NewMessages != null)
						{
							if (this.NewMessages.Invoke(messages.ToArray()))
							{
								sending = true;
								string links = String.Join(",", messages.Select(msg => msg.LINK));
								string statusInfo = "Сообщение доставляется.";
								string statusDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
								string sql = $"UPDATE {Database.Tables.MESSAGES} SET STATUS='{MessageStatus.SENDING}', STATUS_INFO='{statusInfo}', STATUS_DATE='{statusDate}' WHERE LINK IN ({links})";
								int count = _dataAdapter.ExecuteUpdate(sql);
							}
						}
					}
				}
				catch (Exception ex)
				{
					var error = new InvalidOperationException("Ошибка сканирования новых сообщений.", ex);
					_logger.LogError(ex);
					//this.Error?.Invoke(error);
				}
				finally
				{
					if (_started)
					{
						if (sending)
							_queryTimer.Interval = 1;
						else
							_queryTimer.Interval = _interval.TotalMilliseconds;

						_queryTimer.Start();
					}
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
		#endregion


		#region IDisposable
		private bool _disposed = false;

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
			if (_disposed)
				return;

			if (disposing)
			{
				_queryTimer.Dispose();
				_dataAdapter = null;
				_logger = null;
			}

			_disposed = true;
		}

		/// <summary>
		/// Деструктор.
		/// </summary>
		~DatabaseMessageScanner()
		{
			Dispose(false);
		}
		#endregion

	}
}
