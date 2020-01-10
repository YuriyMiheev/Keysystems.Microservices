using System;

namespace Microservices.Bus.Web.VMO
{
	/// <summary>
	/// Статус канала.
	/// </summary>
	public class ChannelStatus
	{
		/// <summary>
		/// 
		/// </summary>
		public bool Created { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool Opened { get; set; }

		/// <summary>
		/// {Get}
		/// </summary>
		public bool Running { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool? Online { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Exception Error { get; set; }
	}
}
