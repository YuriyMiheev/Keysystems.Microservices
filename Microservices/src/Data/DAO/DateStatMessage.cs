using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservices.Data.DAO
{
	/// <summary>
	/// Часть полей сообщения.
	/// </summary>
	public class DateStatMessage
	{

		#region Properties
		/// <summary>
		/// {Get,Set} Внутренний ID.
		/// </summary>
		public virtual int LINK { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Channel { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual DateTime? Date { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Status { get; set; }
		#endregion

	}
}
