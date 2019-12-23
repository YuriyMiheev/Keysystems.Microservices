using System.Collections.Generic;

namespace Microservices.Data.DAO
{
	/// <summary>
	/// Информация о контакте.
	/// </summary>
	public class Contact
	{
		/// <summary>
		/// {Get,Set} Внутренний ID.
		/// </summary>
		public virtual int LINK { get; set; }

		/// <summary>
		/// {Get,Set} Вычисляемый по Address числовой идентификатор.
		/// </summary>
		public virtual int ContactID { get; set; }

		/// <summary>
		/// {Get,Set} Тип канала.
		/// </summary>
		public virtual string Type { get; set; }

		/// <summary>
		/// {Get,Set} Отображаемое имя.
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// {Get,Set} Виртуальный адрес канала.
		/// </summary>
		public virtual string Address { get; set; }

		/// <summary>
		/// {Get,Set} Включен/Выключен.
		/// </summary>
		public virtual bool? Enabled { get; set; }

		/// <summary>
		/// {Get,Set} Контакт системного канала сервиса.
		/// </summary>
		public virtual bool? IsService { get; set; }

		/// <summary>
		/// {Get,Set} Собственный контакт данного канала.
		/// </summary>
		public virtual bool? IsMyself { get; set; }

		/// <summary>
		/// {Get,Set} Доступен.
		/// </summary>
		public virtual bool? Opened { get; set; }

		/// <summary>
		/// {Get,Set} На связи.
		/// </summary>
		public virtual bool? Online { get; set; }

		/// <summary>
		/// {Get,Set} Режим доступа.
		/// </summary>
		public virtual string AccessMode { get; set; }

		/// <summary>
		/// {Get,Set} Комментарий.
		/// </summary>
		public virtual string Comment { get; set; }

		/// <summary>
		/// {Get,Set} Дополнительные св-ва.
		/// </summary>
		public virtual IList<DAO.ContactProperty> Properties { get; set; }
	}
}
