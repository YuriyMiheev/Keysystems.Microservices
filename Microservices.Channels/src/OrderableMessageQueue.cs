using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.Channels
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class OrderableMessageQueue
	{
		private object syncRoot = new object();
		private List<Message> queue;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public OrderableMessageQueue()
		{
			this.queue = new List<Message>();
			this.Enabled = true;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get,Set}
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// {Get}
		/// </summary>
		public int Count
		{
			get
			{
				lock ( this.syncRoot )
				{
					return this.queue.Count;
				}
			}
		}

		private List<int> lastLinks = new List<int>();
		/// <summary>
		/// {Get}
		/// </summary>
		public int[] LastLinks
		{
			get
			{
				lock ( this.syncRoot )
				{
					return this.lastLinks.ToArray();
				}
			}
		}
		#endregion


		#region Methods
		/// <summary>
		/// Order messages by "Priority" and Add not existed N="<paramref name="count"/>" <paramref name="messages"/> to the queue.
		/// </summary>
		/// <param name="messages"></param>
		/// <param name="count"></param>
		/// <returns>True if added messages count > 0.</returns>
		public bool Enqueue(IEnumerable<Message> messages, int count)
		{
			#region Validate parameters
			if ( messages == null )
				throw new ArgumentNullException("messages");

			if ( count < 0 )
				throw new ArgumentException("Параметр должен быть >= 0.", "count");
			#endregion

			if ( messages.Count() == 0 )
				return false;

			if ( count == 0 )
				return false;

			if ( count < 0 )
				count = messages.Count();


			if ( !this.Enabled )
				return false;

			lock ( this.syncRoot )
			{
				if ( !this.Enabled )
					return false;

				var buffer = new SortedDictionary<int, Message>();
				foreach ( Message m in this.queue )
				{
					buffer.Add(m.LINK, m);
				}

				int _count = 0;
				foreach ( Message m in messages )
				{
					if ( !buffer.ContainsKey(m.LINK) && !this.lastLinks.Contains(m.LINK) && (_count < count) )
					{
						buffer.Add(m.LINK, m);
						this.lastLinks.Add(m.LINK);
						_count++;
					}
				}


				List<Message> low = buffer.Values.Where(m => m.Priority < 0).OrderByDescending(m => m.Priority).ToList();
				List<Message> nil = buffer.Values.Where(m => m.Priority == null).ToList();
				List<Message> zero = buffer.Values.Where(m => m.Priority == 0).ToList();
				List<Message> high = buffer.Values.Where(m => m.Priority > 0).OrderByDescending(m => m.Priority).ToList();

				var temp = new List<Message>();
				temp.AddRange(high);
				temp.AddRange(zero);
				temp.AddRange(nil);
				temp.AddRange(low);

				this.queue.Clear();
				this.queue.AddRange(temp);

				return (_count > 0);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool TryDequeue(out Message msg)
		{
			msg = null;

			if ( !this.Enabled )
				return false;

			lock ( this.syncRoot )
			{
				if ( !this.Enabled )
					return false;

				msg = this.queue.FirstOrDefault();
				if ( msg != null )
					return this.queue.Remove(msg);
				else
					return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool TryPeek(out Message msg)
		{
			msg = null;

			if ( !this.Enabled )
				return false;

			lock ( this.syncRoot )
			{
				if ( !this.Enabled )
					return false;

				msg = this.queue.FirstOrDefault();
				return (msg != null);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool Remove(Message msg)
		{
			lock ( this.syncRoot )
			{
				return this.queue.Remove(msg);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			lock ( this.syncRoot )
			{
				this.queue.Clear();
				this.lastLinks.Clear();
			}
		}
		#endregion

	}
}
