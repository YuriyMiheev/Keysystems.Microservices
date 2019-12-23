using System;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Параметры обновления информации о канале.
	/// </summary>
	[Serializable]
	public class ChannelInfoUpdateParams
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public ChannelInfoUpdateParams()
		{ }
		#endregion


		#region Properties
		/// <summary>
		/// {Get,Set} Имя канала.
		/// </summary>
		public string Name { get; set; }

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
		/// {Get,Set} Включен/Выключен.
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// {Get,Set} Режим доступа.
		/// </summary>
		public string AccessMode { get; set; }

		/// <summary>
		/// {Get,Set} Комментарий.
		/// </summary>
		public string Comment { get; set; }
		#endregion

	}
}
