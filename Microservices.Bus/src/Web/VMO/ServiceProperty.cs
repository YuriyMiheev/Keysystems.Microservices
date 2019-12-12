using System;
using System.ComponentModel.DataAnnotations;

namespace Microservices.Bus.VMO
{
	/// <summary>
	/// Дополнительное свойство сервиса.
	/// </summary>
	[Serializable]
	public class ServiceProperty
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public ServiceProperty()
		{ }
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public int LINK { get; set; }

		/// <summary>
		/// {Get} Ссылка на сервис.
		/// </summary>
		public int ServiceLINK { get; set; }

		/// <summary>
		/// {Get,Set} Имя.
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "Не указано имя свойства (Name).")]
		[StringLength(255, ErrorMessage = "Имя свойства (Name) не должно превышать 255 знаков.")]
		public string Name { get; set; }

		/// <summary>
		/// {Get,Set} Значение.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// {Get,Set} Тип.
		/// </summary>
		[StringLength(255, ErrorMessage = "Тип свойства (Type) не должен превышать 255 знаков.")]
		public string Type { get; set; }

		/// <summary>
		/// {Get,Set} Формат.
		/// </summary>
		[StringLength(255, ErrorMessage = "Формат свойства (Format) не должен превышать 255 знаков.")]
		public string Format { get; set; }

		/// <summary>
		/// {Get,Set} Комментарий.
		/// </summary>
		[StringLength(1024, ErrorMessage = "Комментарий (Comment) не должен превышать 1024 знака.")]
		public string Comment { get; set; }
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("{0}={1}", this.Name, this.Value);
		}
		#endregion

	}
}
