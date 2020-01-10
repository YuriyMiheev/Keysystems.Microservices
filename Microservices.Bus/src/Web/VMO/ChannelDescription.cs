using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.Bus.Web.VMO
{
	/// <summary>
	/// Описание канала.
	/// </summary>
	public class ChannelDescription
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public ChannelDescription()
		{
			this.Properties = new List<ChannelDescriptionProperty>();
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get,Set} Провайдер.
		/// </summary>
		public string Provider { get; set; }

		/// <summary>
		/// {Get,Set} Имя типа и сборки, реализующих канал.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// {Get,Set} Версия.
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		/// {Get,Set} Физический адрес.
		/// </summary>
		public string RealAddress { get; set; }

		/// <summary>
		/// {Get,Set} Таймаут (сек).
		/// </summary>
		public int Timeout { get; set; }

		/// <summary>
		/// {Get,Set} Комментарий.
		/// </summary>
		public string Comment { get; set; }

		/// <summary>
		/// {Get,Set} Может обновлять список контактов.
		/// </summary>
		public bool CanSyncContacts { get; set; }

		/// <summary>
		/// {Get,Set} Имя файла иконки.
		/// </summary>
		public string IconName { get; set; }

		///// <summary>
		///// {Get,Set} Css имя иконки.
		///// </summary>
		//public string IconCss { get; set; }

		///// <summary>
		///// {Get,Set} Находится в отдельном AppDomain.
		///// </summary>
		//public bool IsolatedDomain { get; set; }

		/// <summary>
		/// {Get,Set} Путь к исполняемым файлам.
		/// </summary>
		public string BinPath { get; set; }

		/// <summary>
		/// {Get,Set} Поддержка множества экземпляров.
		/// </summary>
		public bool AllowMultipleInstances { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public List<ChannelDescriptionProperty> Properties { get; set; }
		#endregion

	}
}
