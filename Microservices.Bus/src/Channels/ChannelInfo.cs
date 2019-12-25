using System.Collections.Generic;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Информация о канале.
	/// </summary>
	public class ChannelInfo
	{
		private readonly IDictionary<string, ChannelInfoProperty> _properties;


		public ChannelInfo()
		{
			_properties = new Dictionary<string, ChannelInfoProperty>();
		}


		/// <summary>
		/// Доп. св-ва канала.
		/// </summary>
		public IDictionary<string, ChannelInfoProperty> Properties => _properties;

		/// <summary>
		/// ID в таблице списка каналов.
		/// </summary>
		public int LINK { get; set; }

		/// <summary>
		/// Признак блокировки.
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// Провайдер.
		/// </summary>
		public string Provider { get; set; }

		/// <summary>
		/// Тип/Имя исполняемого файла.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Комментарий.
		/// </summary>
		public string Comment { get; set; }

		/// <summary>
		/// Пользовательское имя канала.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Виртуальный адрес.
		/// </summary>
		public string VirtAddress { get; set; }

		/// <summary>
		/// Реальный физический адрес.
		/// </summary>
		public string RealAddress { get; set; }

		/// <summary>
		/// Http адрес.
		/// </summary>
		public string SID { get; set; }

		/// <summary>
		/// Таймаут подключения.
		/// </summary>
		public int Timeout { get; set; }

		/// <summary>
		/// Пароль на входящее подключение.
		/// </summary>
		public string PasswordIn { get; set; }

		/// <summary>
		/// Пароль на исходящее подключение.
		/// </summary>
		public string PasswordOut { get; set; }
	}
}
