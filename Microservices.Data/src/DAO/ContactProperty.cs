using System;

namespace Microservices.Data.DAO
{
	/// <summary>
	/// Дополнительное свойство контакта.
	/// </summary>
	public class ContactProperty
	{

		#region Properties
		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual int LINK { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual Contact Contact { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Value { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Type { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Format { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Comment { get; set; }
		#endregion

	}
}
