using System;
using System.Diagnostics;
using System.Net.Mime;

namespace Microservices
{
	/// <summary>
	/// Дополнительное свойство сообщения.
	/// </summary>
	public class MessageProperty
	{

		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public MessageProperty()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msgLink"></param>
		public MessageProperty(int msgLink)
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
		/// {Get,Set} Значение.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// {Get,Set} Тип.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// {Get,Set} Формат.
		/// </summary>
		public string Format { get; set; }

		/// <summary>
		/// {Get,Set} Комментарий.
		/// </summary>
		public string Comment { get; set; }
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ContentType ContentType()
		{
			try
			{
				if ( String.IsNullOrEmpty(this.Type) )
					return null;

				return new ContentType(this.Type);
			}
			catch ( Exception ex )
			{
				Debug.WriteLine(ex);
				return null;
			}
		}

		/// <summary>
		/// Сравнение объектов по "Name".
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			MessageProperty prop = obj as MessageProperty;
			if ( prop == null )
				return false;

			return (this.Name == prop.Name);
		}

		/// <summary>
		/// Hash код по "Name".
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (this.Name ?? "").GetHashCode();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("{0}={1}", this.Name, this.Value);
		}
		#endregion

	}
}
