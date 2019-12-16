using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Информация о канале.
	/// </summary>
	[DebuggerDisplay("{this.Id}")]
	public class ChannelInfo
	{
		private List<ChannelProperty> _properties;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public ChannelInfo()
		{
			_properties = new List<ChannelProperty>();
		}
		#endregion


		#region Events
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler<EventArgs> StatusChanged;

		private void OnStatusChanged()
		{
			EventHandler<EventArgs> action = this.StatusChanged;
			if ( action != null )
				action(this, EventArgs.Empty);
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Идентификатор канала вида "#{LINK} ({VirtAddress})".
		/// </summary>
		public string Id
		{
			get { return String.Format("#{0} ({1})", this.LINK, this.VirtAddress); }
		}

		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public int LINK { get; set; }

		/// <summary>
		/// {Get,Set} Имя канала.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// {Get} Провайдер.
		/// </summary>
		public string Provider { get; internal set; }

		private string virtAddress;
		/// <summary>
		/// {Get,Set} Виртуальный адрес.
		/// </summary>
		public string VirtAddress
		{
			get { return (virtAddress ?? "").ToLower(); }
			set { virtAddress = (value ?? "").ToLower(); }
		}

		/// <summary>
		/// {Get} SID канала.
		/// </summary>
		public string SID { get; set; }

		/// <summary>
		/// {Get,Set} Физический адрес.
		/// </summary>
		public string RealAddress { get; set; }

		/// <summary>
		/// {Get,Set} Пароль на прием.
		/// </summary>
		public string PasswordIn { get; set; }

		/// <summary>
		/// {Get,Set} Пароль на отправку.
		/// </summary>
		public string PasswordOut { get; set; }

		/// <summary>
		/// {Get,Set} Таймаут (сек).
		/// </summary>
		public int Timeout { get; set; }

		/// <summary>
		/// {Get} Канал системный.
		/// </summary>
		public bool IsSystem
		{
			get { return (this.Provider == "SYSTEM"); }
		}

		/// <summary>
		/// {Get,Set} Включен/Выключен.
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// {Get,Set} Комментарий.
		/// </summary>
		public string Comment { get; set; }

		private bool opened;
		/// <summary>
		/// {Get,Set} Канал открыт.
		/// </summary>
		public bool Opened
		{
			get { return opened; }
			set
			{
				if ( value != opened )
				{
					opened = value;
					OnStatusChanged();
				}
			}
		}

		private bool running;
		/// <summary>
		/// {Get,Set} Канал запущен.
		/// </summary>
		public bool Running
		{
			get { return running; }
			set
			{
				if ( value != running )
				{
					running = value;
					OnStatusChanged();
				}
			}
		}

		private bool? online;
		/// <summary>
		/// {Get,Set} Канал доступен.
		/// </summary>
		public bool? Online
		{
			get { return online; }
			set
			{
				if ( value != online )
				{
					online = value;
					OnStatusChanged();
				}
			}
		}

		//private string accessMode;
		///// <summary>
		///// {Get,Set} Режим доступа.
		///// </summary>
		//public string AccessMode
		//{
		//	get { return (String.IsNullOrEmpty(accessMode) ? Channels.AccessMode.FULL : accessMode); }
		//	set { accessMode = value; }
		//}

		/// <summary>
		/// {Get,Set} Ошибка.
		/// </summary>
		public ExceptionWrapper Error { get; set; }

		/// <summary>
		/// {Get} Дополнительные свойства.
		/// </summary>
		public ChannelProperty[] Properties
		{
			get { return _properties.ToArray(); }
			internal set { _properties = (value ?? new ChannelProperty[0]).ToList(); }
		}

		///// <summary>
		///// {Get} Настройки канала.
		///// </summary>
		//public ChannelSettings ChannelSettings
		//{
		//	get { return new ChannelSettings(this.Properties); }
		//}

		///// <summary>
		///// {Get} Настройки обработки сообщений.
		///// </summary>
		//public MessageSettings MessageSettings
		//{
		//	get { return new MessageSettings(this.Properties); }
		//}

		/// <summary>
		/// {Get} Канал может синхронизировать контакты.
		/// </summary>
		public bool CanSyncContacts
		{
			get
			{
				//if ( this.AccessMode == Channels.AccessMode.VIEW )
				//	return false;

				if ( this.Description == null )
					return false;

				return this.Description.CanSyncContacts;
			}
		}

		//private bool? isolatedDomain;
		///// <summary>
		///// {Get} Канал запущен в отдельном домене.
		///// </summary>
		//public bool IsolatedDomain
		//{
		//	get
		//	{
		//		if ( this.Description != null )
		//			isolatedDomain = this.Description.IsolatedDomain;

		//		isolatedDomain = (isolatedDomain == null || isolatedDomain.Value == false ? new Nullable<bool>() : true);
		//		return (isolatedDomain ?? false);
		//	}
		//}

		/// <summary>
		/// {Get} Описание канала.
		/// </summary>
		public ChannelDescription Description { get; private set; }
		#endregion


		#region Methods
		/// <summary>
		/// Добавление нового св-ва.
		/// </summary>
		/// <param name="prop"></param>
		public void AddNewProperty(ChannelProperty prop)
		{
			#region Validate parameters
			if ( prop == null )
				throw new ArgumentNullException("prop");

			if ( String.IsNullOrEmpty(prop.Name) )
				throw new ArgumentException("Отсутствует имя свойства.", "prop");

			if ( prop.LINK != 0 )
				throw new ArgumentException("Свойство должно иметь LINK = 0.", "prop");
			#endregion

			if (_properties.Any(p => p.Name == prop.Name) )
				throw new InvalidOperationException(String.Format("Канал уже содержит свойство {0}.", prop.Name));

			prop.ChannelLINK = this.LINK;
			_properties.Add(prop);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propName"></param>
		/// <returns></returns>
		public ChannelProperty FindProperty(string propName)
		{
			#region Validate parameters
			if ( String.IsNullOrEmpty(propName) )
				throw new ArgumentException("propName");
			#endregion

			return _properties.SingleOrDefault(prop => prop.Name == propName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propName"></param>
		/// <returns></returns>
		public ChannelProperty GetProperty(string propName)
		{
			#region Validate parameters
			if ( String.IsNullOrEmpty(propName) )
				throw new ArgumentException("propName");
			#endregion

			ChannelProperty prop = FindProperty(propName);
			if ( prop == null )
				throw new InvalidOperationException(String.Format("Свойство \"{0}\" не найдено.", propName));

			return prop;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propName"></param>
		/// <param name="updateParams"></param>
		public void UpdateProperty(string propName, ChannelPropertyUpdateParams updateParams)
		{
			#region Validate parameters
			if ( String.IsNullOrEmpty(propName) )
				throw new ArgumentException("propName");

			if ( updateParams == null )
				throw new ArgumentNullException("updateParams");
			#endregion

			ChannelProperty prop = GetProperty(propName);
			if ( prop.ReadOnly )
				throw new InvalidOperationException(String.Format("Свойство \"{0}\" только для чтения.", propName));

			prop.Value = updateParams.Value;
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

			ChannelProperty prop = FindProperty(propName);
			if ( prop != null )
				_properties.Remove(prop);
		}

		/// <summary>
		/// 
		/// </summary>
		public void ClearProperties()
		{
			_properties.Clear();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="description"></param>
		public void SetDescription(ChannelDescription description)
		{
			#region Validate parameters
			if ( description == null )
				throw new ArgumentNullException("description");
			#endregion

			if ( this.Description != null )
				throw new InvalidOperationException("Описание канала уже задано.");

			this.Description = description;
			this.Provider = description.Provider;

			List<string> existProps = _properties.Select(p => p.Name).Where(prop => description.Properties.Select(p => p.Name).Contains(prop)).ToList();
			List<string> delProps = _properties.Select(p => p.Name).Where(prop => !description.Properties.Select(p => p.Name).Contains(prop)).ToList();
			List<string> newProps = description.Properties.Select(p => p.Name).Where(prop => !_properties.Select(p => p.Name).Contains(prop)).ToList();

			existProps.ForEach(p =>
				{
					ChannelDescriptionProperty dp = description.GetProperty(p);
					ChannelProperty prop = GetProperty(p);
					string value = prop.Value;
					dp.CopyTo(prop);
					prop.Value = value;
				});
			delProps.ForEach(p => RemoveProperty(p));
			newProps.ForEach(p =>
				{
					ChannelDescriptionProperty dp = description.GetProperty(p);
					var prop = new ChannelProperty();
					dp.CopyTo(prop);
					AddNewProperty(prop);
				});
		}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="propName"></param>
		///// <returns></returns>
		//public string PropertyValue(string propName)
		//{
		//	ChannelProperty prop = FindProperty(propName);
		//	if ( prop == null )
		//		return null;

		//	if ( prop.Value != null )
		//		prop.Value = prop.Value.Trim();

		//	return prop.Value;
		//}

		/// <summary>
		/// Сравнение объектов по "VirtAddress".
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			ChannelInfo channel = obj as ChannelInfo;
			if ( channel == null )
				return false;

			return this.VirtAddress.Equals(channel.VirtAddress, StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Hash код объекта по "VirtAddress".
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.VirtAddress.GetHashCode();
		}
		#endregion

	}
}
