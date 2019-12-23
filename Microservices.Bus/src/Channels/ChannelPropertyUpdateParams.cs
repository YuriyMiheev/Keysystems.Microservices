using System;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Параметры обновления свойства канала.
	/// </summary>
	[Serializable]
	public sealed class ChannelPropertyUpdateParams
	{

		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public ChannelPropertyUpdateParams()
		{ }
		#endregion


		#region Properties
		///// <summary>
		///// {Get,Set} Имя.
		///// </summary>
		//public string Name { get; set; }

		/// <summary>
		/// {Get,Set} Значение.
		/// </summary>
		public string Value { get; set; }

		///// <summary>
		///// {Get,Set} Тип.
		///// </summary>
		//public string Type { get; set; }

		///// <summary>
		///// {Get,Set} Формат.
		///// </summary>
		//public string Format { get; set; }

		///// <summary>
		///// {Get,Set} Комментарий.
		///// </summary>
		//public string Comment { get; set; }
		#endregion

	}
}
