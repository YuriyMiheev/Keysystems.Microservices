using System;
using System.IO;

namespace Microservices
{
	/// <summary>
	/// Тело сообщения.
	/// </summary>
	public class MessageBody : IDisposable
	{

		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public MessageBody()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		public MessageBody(int msgLink)
		{
			this.MessageLINK = msgLink;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get,Set} ID сообщения.
		/// </summary>
		public int MessageLINK { get; set; }

		/// <summary>
		/// {Get,Set} Имя.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// {Get,Set} Тип.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// {Get,Set} Длина.
		/// </summary>
		public int? Length { get; set; }

		/// <summary>
		/// {Get,Set} Размер.
		/// </summary>
		public int? FileSize { get; set; }

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
			return String.Format("#{0} ({1})", this.MessageLINK, this.Name);
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
		~MessageBody()
		{
			Dispose(false);
		}
		#endregion

	}
}
