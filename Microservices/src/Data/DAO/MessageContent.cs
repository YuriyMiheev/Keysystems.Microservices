namespace Microservices.Data.DAO
{
	/// <summary>
	/// Содержимое сообщения.
	/// </summary>
	public class MessageContent
	{
		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual int LINK { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual Message Message { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Type { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual int? Length { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual int? FileSize { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Value { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Comment { get; set; }
	}
}
