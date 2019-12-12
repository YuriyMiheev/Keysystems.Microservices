using System;

namespace Keysystems.Data.DAO
{
	/// <summary>
	/// Посылка сообщения.
	/// </summary>
	public class MessagePost
	{

		#region Properties
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
		public virtual string Address { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual int RetryCount { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual MessageStatus Status { get; set; }
		#endregion

	}
}
