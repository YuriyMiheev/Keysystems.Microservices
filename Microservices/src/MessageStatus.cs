using System;

namespace Microservices
{
	/// <summary>
	/// Статусы сообщений и посылок.
	/// </summary>
	public class MessageStatus
	{

		#region Statuses
		/// <summary>
		/// {Get} Возможные статусы.
		/// </summary>
		public static string[] Statuses
		{
            get { return new string[] { NULL, DRAFT, NEW, SENDING, RECEIVING, DELIVERED, COMPLETED, ERROR, DELETED, WAITING }; }
		}


		/// <summary>
		/// Черновик (null).
		/// </summary>
		public const string NULL = null;

		/// <summary>
		/// Черновик (DRAFT).
		/// </summary>
		public const string DRAFT = "DRAFT";

		/// <summary>
		/// Новое.
		/// </summary>
		public const string NEW = "NEW";

		/// <summary>
		/// Отправляется.
		/// </summary>
		public const string SENDING = "SENDING";

		/// <summary>
		/// Принимается.
		/// </summary>
		public const string RECEIVING = "RECEIVING";

		/// <summary>
		/// Доставлено (для асинхронных сообщений).
		/// </summary>
		public const string DELIVERED = "DELIVERED";

		/// <summary>
		/// Завершено.
		/// </summary>
		public const string COMPLETED = "COMPLETED";

		/// <summary>
		/// Ошибка.
		/// </summary>
		public const string ERROR = "ERROR";

		/// <summary>
		/// Удалено.
		/// </summary>
		public const string DELETED = "DELETED";

		/// <summary>
		/// В ожидании.
		/// </summary>
        public const string WAITING = "WAITING";
		#endregion


		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public MessageStatus()
		{ }
		#endregion


		#region Properties
		private string _value;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		public string Value
		{
			get { return _value; }
			set { _value = (value == MessageStatus.DRAFT ? null : value); }
		}

		/// <summary>
		/// {Get,Set} Дата и время.
		/// </summary>
		public DateTime? Date { get; set; }

		/// <summary>
		/// {Get,Set} Информация.
		/// </summary>
		public string Info { get; set; }

		/// <summary>
		/// {Get,Set} Код статуса.
		/// </summary>
		public int? Code { get; set; }
		#endregion


		#region IsStatus
		/// <summary>
		/// {Get} Черновик.
		/// </summary>
		public bool IsDraft
		{
			get { return String.IsNullOrWhiteSpace(this.Value) || this.Value == MessageStatus.DRAFT; }
		}

		/// <summary>
		/// {Get} Новое.
		/// </summary>
		public bool IsNew
		{
			get { return (this.Value == MessageStatus.NEW); }
		}

		/// <summary>
		/// {Get} Доставлено.
		/// </summary>
		public bool IsDelivered
		{
			get { return (this.Value == MessageStatus.DELIVERED); }
		}

		/// <summary>
		/// {Get} Ошибочное.
		/// </summary>
		public bool IsError
		{
			get { return (this.Value == MessageStatus.ERROR); }
		}

		/// <summary>
		/// {Get} Успешно завершенное.
		/// </summary>
		public bool IsCompleted
		{
			get { return (this.Value == MessageStatus.COMPLETED); }
		}

		/// <summary>
		/// {Get} Отправляется.
		/// </summary>
		public bool IsSending
		{
			get { return (this.Value == MessageStatus.SENDING); }
		}

		/// <summary>
		/// {Get} Принимается.
		/// </summary>
		public bool IsReceiving
		{
			get { return (this.Value == MessageStatus.RECEIVING); }
		}

		/// <summary>
		/// {Get} Удалено.
		/// </summary>
		public bool IsDeleted
		{
			get { return (this.Value == MessageStatus.DELETED); }
		}

		/// <summary>
		/// {Get} Удалено.
		/// </summary>
		public bool IsWaiting
		{
			get { return (this.Value == MessageStatus.WAITING); }
		}
		#endregion


		#region Methods
		/// <summary>
		/// Сравнение по Value.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if ( obj == null )
				return false;

			//if ( base.Equals(obj) )
			//   return true;

			MessageStatus status = (MessageStatus)obj;
			return ((this.Value ?? "").Equals((status.Value ?? ""), StringComparison.InvariantCultureIgnoreCase));
		}

		/// <summary>
		/// Hash код по Value.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (this.Value ?? "").GetHashCode();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Value;
		}
		#endregion

	}
}
