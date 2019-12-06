using System;
using System.IO;

namespace Microservices
{
	/// <summary>
	/// Контент сообщения.
	/// </summary>
	public class MessageContent : IDisposable
	{

		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public MessageContent()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		public MessageContent(int msgLink)
		{
			this.MessageLINK = msgLink;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public int LINK { get; set; }

		/// <summary>
		/// {Get} Ссылка на сообщение.
		/// </summary>
		public int? MessageLINK { get; set; }

		/// <summary>
		/// {Get,Set} Имя.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// {Get,Set} Тип.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// {Get,Set} Фактический размер.
		/// </summary>
		public int? Length { get; set; }

		/// <summary>
		/// {Get,Set} Исходный размер.
		/// </summary>
		public int? FileSize { get; set; }

		/// <summary>
		/// {Get,Set} Комментарий.
		/// </summary>
		public string Comment { get; set; }

		private TextReader _value;
		/// <summary>
		/// {Get,Set} Значение.
		/// </summary>
		public TextReader Value
		{
			get { return _value; }
			set { _value = value; }
		}
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("#{0} ({1})", this.LINK, this.Name);
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

		private void Dispose(bool disposing)
		{
			if ( disposed )
				return;

			if ( disposing )
			{
				if ( this.Value != null )
					this.Value.Dispose();
			}

			disposed = true;
		}

		/// <summary>
		/// Деструктор.
		/// </summary>
		~MessageContent()
		{
			Dispose(false);
		}
		#endregion

	}
}
