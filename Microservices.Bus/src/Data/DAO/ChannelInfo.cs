using System.Collections.Generic;

namespace Microservices.Bus.Data.DAO
{
	/// <summary>
	/// Информация о канале.
	/// </summary>
	public class ChannelInfo
	{
		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public virtual int LINK { get; set; }

		/// <summary>
		/// {Get,Set} Имя канала.
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// {Get,Set} Провайдер.
		/// </summary>
		public virtual string Provider { get; set; }

		/// <summary>
		/// {Get,Set} Виртуальный адрес.
		/// </summary>
		public virtual string VirtAddress { get; set; }

		///// <summary>
		///// {Get,Set} Канал запущен в отдельном домене.
		///// </summary>
		//public virtual bool? IsolatedDomain { get; set; }

		/// <summary>
		/// {Get,Set} SID канала.
		/// </summary>
		public virtual string SID { get; set; }

		/// <summary>
		/// {Get,Set} Физический адрес.
		/// </summary>
		public virtual string RealAddress { get; set; }

		/// <summary>
		/// {Get,Set} Пароль на прием.
		/// </summary>
		public virtual string PasswordIn { get; set; }

		/// <summary>
		/// {Get,Set} Пароль на отправку.
		/// </summary>
		public virtual string PasswordOut { get; set; }

		/// <summary>
		/// {Get,Set} Таймаут (сек).
		/// </summary>
		public virtual int? Timeout { get; set; }

		/// <summary>
		/// {Get,Set} Включен/Выключен.
		/// </summary>
		public virtual bool? Enabled { get; set; }

		///// <summary>
		///// {Get,Set} Режим доступа.
		///// </summary>
		//public virtual string AccessMode { get; set; }

		/// <summary>
		/// {Get,Set} Комментарий.
		/// </summary>
		public virtual string Comment { get; set; }

		/// <summary>
		/// {Get,Set} Дополнительные свойства.
		/// </summary>
		public virtual IList<DAO.ChannelInfoProperty> Properties { get; set; }
	}
}
