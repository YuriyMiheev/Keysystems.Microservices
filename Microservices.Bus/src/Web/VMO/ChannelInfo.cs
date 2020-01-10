using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Microservices.Bus.Web.VMO
{
	/// <summary>
	/// Информация о канале.
	/// </summary>
	[Serializable]
	public class ChannelInfo
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public ChannelInfo()
		{
			this.Properties = new List<ChannelInfoProperty>();
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public int LINK { get; set; }

		/// <summary>
		/// {Get,Set} Имя канала.
		/// </summary>
		[StringLength(255, ErrorMessage = "Имя канала (Name) не должно превышать 255 знаков.")]
		public string Name { get; set; }

		/// <summary>
		/// {Get,Set} Провайдер.
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "Не указан провайдер канала (Provider).")]
		[StringLength(50, ErrorMessage = "Имя провайдера (Provider) не должно превышать 50 знаков.")]
		public string Provider { get; set; }

		/// <summary>
		/// {Get,Set} Виртуальный адрес.
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "Не указан виртуальный адрес канала (VirtAddress).")]
		[StringLength(255, ErrorMessage = "Виртуальный адрес (VirtAddress) не должен превышать 255 знаков.")]
		public string VirtAddress { get; set; }

		/// <summary>
		/// {Get,Set} SID.
		/// </summary>
		public string SID { get; set; }

		/// <summary>
		/// {Get,Set} Физический адрес.
		/// </summary>
		[StringLength(1024, ErrorMessage = "Физический адрес (RealAddress) не должен превышать 1024 знака.")]
		public string RealAddress { get; set; }

		/// <summary>
		/// {Get,Set} Пароль на прием.
		/// </summary>
		[StringLength(255, ErrorMessage = "Пароль на прием (PasswordIn) не должен превышать 255 знаков.")]
		public string PasswordIn { get; set; }

		/// <summary>
		/// {Get,Set} Пароль на отправку.
		/// </summary>
		[StringLength(255, ErrorMessage = "Пароль на отправку (PasswordOut) не должен превышать 255 знаков.")]
		public string PasswordOut { get; set; }

		/// <summary>
		/// {Get,Set} Таймаут.
		/// </summary>
		public int? Timeout { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public bool IsSystem { get; set; }

		/// <summary>
		/// {Get,Set} Включен/Выключен.
		/// </summary>
		public bool? Enabled { get; set; }

		/// <summary>
		/// {Get,Set} Комментарий.
		/// </summary>
		[StringLength(1024, ErrorMessage = "Комментарий (Comment) не должен превышать 1024 знака.")]
		public string Comment { get; set; }

		///// <summary>
		///// {Get,Set}
		///// </summary>
		//public bool Opened { get; set; }

		///// <summary>
		///// {Get,Set}
		///// </summary>
		//public bool Running { get; set; }

		///// <summary>
		///// {Get,Set}
		///// </summary>
		//public bool? Online { get; set; }

		///// <summary>
		///// {Get,Set} Режим.
		///// </summary>
		//[StringLength(50, ErrorMessage = "Режим (AccessMode) не должен превышать 50 знаков.")]
		//public string AccessMode { get; set; }

		///// <summary>
		///// {Get,Set} Ошибка.
		///// </summary>
		//public ExceptionWrapper Error { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public List<ChannelInfoProperty> Properties { get; set; }

		///// <summary>
		///// {Get,Set}
		///// </summary>
		//public bool CanSyncContacts { get; set; }

		///// <summary>
		///// {Get,Set}
		///// </summary>
		//public bool IsolatedDomain { get; set; }
		#endregion

	}
}
