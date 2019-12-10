using System;
using System.Diagnostics;

namespace Microservices.Bus
{
	/// <summary>
	/// Дополнительное свойство канала.
	/// </summary>
	[DebuggerDisplay("{this.Name}")]
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
		public string Default { get; set; }

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
