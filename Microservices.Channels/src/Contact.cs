using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.Channels
{
	/// <summary>
	/// Информация о контакте.
	/// </summary>
	public sealed class Contact : MarshalByRefObject
	{

		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public Contact()
		{
			this.properties = new List<ContactProperty>();
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public int LINK { get; set; }

		/// <summary>
		/// {Get} Вычисляемый по Address числовой идентификатор.
		/// </summary>
		public int ContactID
		{
			get { return (this.Address ?? "").GetHashCode(); }
		}

		/// <summary>
		/// {Get,Set} Тип канала.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// {Get,Set} Отображаемое имя.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// {Get,Set} Виртуальный адрес канала.
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// {Get,Set} Включен/Выключен.
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// {Get,Set} Контакт системного канала сервиса.
		/// </summary>
		public bool IsService { get; set; }

		/// <summary>
		/// {Get,Set} Собственный контакт данного канала.
		/// </summary>
		public bool IsMyself { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public bool IsRemote { get; set; }

		/// <summary>
		/// {Get,Set} Доступен.
		/// </summary>
		public bool Opened { get; set; }

		/// <summary>
		/// {Get,Set} На связи.
		/// </summary>
		public bool? Online { get; set; }

		//private string accessMode;
		///// <summary>
		///// {Get,Set} Режим доступа.
		///// </summary>
		//public string AccessMode
		//{
		//	get { return accessMode; }
		//	set { accessMode = (String.IsNullOrEmpty(value) ? Channels.AccessMode.FULL : value); }
		//}

		/// <summary>
		/// {Get,Set} Комментарий.
		/// </summary>
		public string Comment { get; set; }

		private List<ContactProperty> properties;
		/// <summary>
		/// {Get} Дополнительные св-ва.
		/// </summary>
		public ContactProperty[] Properties
		{
			get { return this.properties.ToArray(); }
			internal set { this.properties = value.ToList(); }
		}
		#endregion


		#region Methods
		/// <summary>
		/// Добавление нового св-ва.
		/// </summary>
		/// <param name="prop"></param>
		public void AddProperty(ContactProperty prop)
		{
			#region Validate parameters
			if ( prop == null )
				throw new ArgumentNullException("prop");

			if ( String.IsNullOrEmpty(prop.Name) )
				throw new ArgumentException("Отсутствует имя свойства.", "prop");
			#endregion

			if ( this.Properties.Any(p => p.Name == prop.Name) )
				throw new InvalidOperationException(String.Format("Канал уже содержит свойство {0}.", prop.Name));

			prop.ContactLINK = this.LINK;
			this.properties.Add(prop);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propName"></param>
		/// <returns></returns>
		public ContactProperty FindProperty(string propName)
		{
			#region Validate parameters
			if ( String.IsNullOrEmpty(propName) )
				throw new ArgumentException("propName");
			#endregion

			return this.properties.SingleOrDefault(prop => prop.Name == propName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propName"></param>
		/// <returns></returns>
		public ContactProperty GetProperty(string propName)
		{
			#region Validate parameters
			if ( String.IsNullOrEmpty(propName) )
				throw new ArgumentException("propName");
			#endregion

			ContactProperty prop = FindProperty(propName);
			if ( prop == null )
				throw new InvalidOperationException(String.Format("Свойство \"{0}\" не найдено.", propName));

			return prop;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propName"></param>
		public void RemoveProperty(string propName)
		{
			#region Validate parameters
			if ( String.IsNullOrEmpty(propName) )
				throw new ArgumentException("propName");
			#endregion

			ContactProperty prop = FindProperty(propName);
			if ( prop != null )
				this.properties.Remove(prop);
		}

		/// <summary>
		/// Сравнение объектов по "Address".
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			Contact contact = obj as Contact;
			if ( contact == null )
				return false;

			return this.Address.Equals((contact.Address ?? ""), StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Hash код объекта по "Address".
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (this.Address ?? "").GetHashCode();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("#{0} ({1})", this.LINK, this.Address);
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
