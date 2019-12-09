namespace Microservices.Bus.Data.DAO
{
	/// <summary>
	/// Дополнительное свойство сервиса.
	/// </summary>
	public class ServiceProperty
	{

		#region Properties
		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public virtual int LINK { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public virtual ServiceInfo Service { get; set; }

		/// <summary>
		/// {Get,Set} Имя.
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// {Get,Set} Значение.
		/// </summary>
		public virtual string Value { get; set; }

		/// <summary>
		/// {Get,Set} Значение по умолчанию.
		/// </summary>
		public virtual string DafaultValue { get; set; }

		/// <summary>
		/// {Get,Set} Тип.
		/// </summary>
		public virtual string Type { get; set; }

		/// <summary>
		/// {Get,Set} Формат.
		/// </summary>
		public virtual string Format { get; set; }

		/// <summary>
		/// {Get,Set} Комментарий.
		/// </summary>
		public virtual string Comment { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public virtual bool? ReadOnly { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public virtual bool? Secret { get; set; }
		#endregion

	}
}
