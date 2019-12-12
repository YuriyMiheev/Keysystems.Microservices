using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservices.Data.DAO
{
	/// <summary>
	/// Тело сообщения.
	/// </summary>
	public class MessageBodyInfo
	{

		#region Properties
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
		/// {Get,Set} Фактический размер.
		/// </summary>
		public virtual int? Length { get; set; }

		/// <summary>
		/// {Get,Set} Истинный размер.
		/// </summary>
		public virtual int? FileSize { get; set; }
		#endregion

	}
}
