namespace Microservices.Data.DAO
{
	/// <summary>
	/// Тело сообщения.
	/// </summary>
	public class MessageBody
	{
		/// <summary>
		/// {Get} ID сообщения.
		/// </summary>
		public virtual int MessageLINK { get; set; }

		/// <summary>
		/// {Get,Set} Имя.
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// {Get,Set} Тип.
		/// </summary>
		public virtual string Type { get; set; }

		/// <summary>
		/// {Get,Set} Длина.
		/// </summary>
		public virtual int? Length { get; set; }

		/// <summary>
		/// {Get,Set} Размер.
		/// </summary>
		public virtual int? FileSize { get; set; }

		/// <summary>
		/// {Get,Set} Значение.
		/// </summary>
		public virtual string Value { get; set; }
	}
}
