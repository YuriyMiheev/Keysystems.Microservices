using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
//using System.Threading;
using System.Threading.Tasks.Dataflow;

using NHibernate;
using NHibernate.Criterion;
using Microservices.Channels.Adapters;
using Microservices.Channels.Data;

//using Keysystems.RemoteMessaging.Adapters;
//using Keysystems.RemoteMessaging.Data;
//using Keysystems.RemoteMessaging.Lib;

namespace Microservices.Channels.MSSQL
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class MessageScannerBase : IDisposable
	{
		private MessageDataAdapterBase dataAdapter;
		private Timer queryTimer;
		private TimeSpan queryTimerInterval;
		private Timer readTimer;
		private OrderableMessageQueue messageQueue;
		private int messagePortion = 0;
		private ITargetBlock<Message> target;
		System.Threading.CancellationToken cancellationToken;
		private bool started;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="recipient"></param>
		protected MessageScannerBase(IChannelService channel, string recipient)
		{
			this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
			this.Recipient = recipient ?? throw new ArgumentException("Пустой адрес получателя сообщений.", nameof(recipient));

			this.messagePortion = channel.MessageSettings.ScanPortion;

			this.dataAdapter = channel.MessageDataAdapter;
			this.messageQueue = new OrderableMessageQueue();

			this.queryTimer = new Timer() { AutoReset = false };
			this.queryTimer.Elapsed += new ElapsedEventHandler(queryTimer_Elapsed);

			this.readTimer = new Timer() { AutoReset = false, Interval = 1 };
			this.readTimer.Elapsed += new ElapsedEventHandler(readTimer_Elapsed);
		}
		#endregion


		#region Properties
		/// <summary>
		/// 
		/// </summary>
		protected IChannelService channel;

		/// <summary>
		/// {Get}
		/// </summary>
		public string Recipient { get; private set; }
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scanInterval"></param>
		/// <param name="target"></param>
		/// <param name="cancellationToken"></param>
		public virtual void Start(TimeSpan scanInterval, ITargetBlock<Message> target, System.Threading.CancellationToken cancellationToken = default)
		{
			if ( this.started )
				return;

			lock ( this )
			{
				if ( this.started )
					return;

				this.started = true;

				this.cancellationToken = cancellationToken;
				this.cancellationToken.Register(() =>
					{
						OnCancel();
					});

				this.messageQueue.Enabled = true;

				this.target = target ?? throw new ArgumentNullException("target");

				this.queryTimerInterval = scanInterval;
				this.queryTimer.Interval = scanInterval.TotalMilliseconds;

				this.queryTimer.Start();
				this.readTimer.Start();
			}
		}
		#endregion



		#region Event Handlers
		void queryTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Timer thisTimer = (Timer)sender;

			if ( this.started )
			{
				try
				{
					IDataQuery dataQuery = null;
					try
					{
						dataQuery = this.dataAdapter.OpenQuery();
						var query = CreateOfflineSelectMessagesQuery(this.messageQueue.LastLinks);
						List<Message> messages = query.GetExecutableQueryOver(dataQuery.Session).List().Select(msg => msg.ToObj()).ToList();
						if ( messages.Count > 0 )
						{
							LogTrace(String.Format("Найдено {1} новых сообщений для {0}", this.Recipient, messages.Count));

							PreProcessSelectedMessages(messages, this.cancellationToken);

							this.messageQueue.Enqueue(messages, this.messagePortion);
						}
					}
					catch ( Exception ex )
					{
						throw new InvalidOperationException("Ошибка сканирования новых сообщений.", ex);
					}
					finally
					{
						if ( dataQuery != null )
							dataQuery.Dispose();
					}
				}
				catch ( Exception ex )
				{
					SetError(ex);
					LogError(ex);
				}
				finally
				{
					thisTimer.Interval = this.queryTimerInterval.TotalMilliseconds;

					if ( this.started )
						thisTimer.Start();
				}
			}
		}

		void readTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Timer thisTimer = (Timer)sender;

			if ( this.started )
			{
				Message msg;
				if ( this.messageQueue.TryPeek(out msg) )
				{
					//this.target.SendAsync(msg);
					//this.messageQueue.Remove(msg);

					if ( this.target.Post(msg) )
						this.messageQueue.Remove(msg);
				}

				if ( this.started )
					thisTimer.Start();
			}
		}
		#endregion


		#region Protected
		/// <summary>
		/// 
		/// </summary>
		/// <param name="exceptLinks"></param>
		/// <returns></returns>
		protected abstract QueryOver<DAO.Message, DAO.Message> CreateOfflineSelectMessagesQuery(IEnumerable<int> exceptLinks);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="messages"></param>
		/// <param name="cancellationToken"></param>
		protected virtual void PreProcessSelectedMessages(List<Message> messages, System.Threading.CancellationToken cancellationToken)
		{
			foreach ( Message msg in messages.ToList() )
			{
				if ( msg.TTL != null && msg.TTL >= DateTime.Now )
				{
					messages.Remove(msg);

					try
					{
						msg.SetStatus(MessageStatus.DELETED, "Сообщение устарело.");
						this.dataAdapter.SaveMessage(msg);
					}
					catch { }
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnCancel()
		{
			this.messageQueue.Enabled = false;

			this.readTimer.Stop();
			this.queryTimer.Stop();

			this.messageQueue.Clear();
			this.started = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="error"></param>
		protected virtual void LogError(Exception error)
		{
			if ( this.channel != null )
				this.channel.LogError(error);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		protected virtual void LogTrace(string text)
		{
				this.channel.LogTrace(text);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="error"></param>
		protected virtual void SetError(Exception error)
		{
				this.channel.SetError(error);
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
			if ( disposed )
				return;

			if ( disposing )
			{
				this.queryTimer.Dispose();
				this.readTimer.Dispose();

				this.target = null;
				this.channel = null;
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
