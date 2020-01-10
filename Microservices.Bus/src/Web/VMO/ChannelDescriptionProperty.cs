using System;

namespace Microservices.Bus.Web.VMO
{
	/// <summary>
	/// Дополнительное свойство канала.
	/// </summary>
	public class ChannelDescriptionProperty
	{

		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public ChannelDescriptionProperty()
		{ }
		#endregion


		#region Properties
		/// <summary>
		/// {Get,Set} Имя.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// {Get,Set} Значение.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// {Get,Set} Значение по умолчанию.
		/// </summary>
		public string DefaultValue { get; set; }

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

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public bool ReadOnly { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public bool Secret { get; set; }
		#endregion

	}
}
