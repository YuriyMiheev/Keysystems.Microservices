using System;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Дополнительное свойство канала.
	/// </summary>
	[Serializable]
	public class ChannelProperty
	{

		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		public ChannelProperty()
		{ }
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public int LINK { get; internal set; }

		/// <summary>
		/// {Get} Ссылка на канал.
		/// </summary>
		public int? ChannelLINK { get; internal set; }

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


		#region Methods
		/// <summary>
		/// Сравнение объектов по Name.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			ChannelProperty prop = obj as ChannelProperty;
			if ( prop == null )
				return false;

			return (this.Name == prop.Name);
		}

		/// <summary>
		/// Hash код объекта по Name.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (this.Name ?? "").GetHashCode();
		}

		/// <summary>
		/// Возвращает строку Name=Value.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("{0}={1}", this.Name, this.Value);
		}
		#endregion

	}
}
