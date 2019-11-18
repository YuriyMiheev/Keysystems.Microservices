using System;

namespace Microservices.Channels.DAO
{
	/// <summary>
	/// Дополнительное свойство контакта.
	/// </summary>
	public class ContactProperty : MarshalByRefObject
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


		#region MarshalByRefObject
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override object InitializeLifetimeService()
		{
			return null;
		}
		#endregion

	}
}
