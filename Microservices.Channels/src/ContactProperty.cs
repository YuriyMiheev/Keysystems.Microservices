using System;

namespace Microservices.Channels
{
	/// <summary>
	/// Дополнительное свойство контакта.
	/// </summary>
	public class ContactProperty : MarshalByRefObject
	{

		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public ContactProperty()
		{ }
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public int LINK { get; set; }

		/// <summary>
		/// {Get} Ссылка на контакт.
		/// </summary>
		public int? ContactLINK { get; set; }

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
		/// Сравнение объектов по "Name".
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			ContactProperty prop = obj as ContactProperty;
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
		/// Возвращает "Name=Value".
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("{0}={1}", this.Name, this.Value);
		}
		#endregion


		#region MarshalByRefObject
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override object InitializeLifetimeService()
		{
			return null;
		}
		#endregion

	}
}
