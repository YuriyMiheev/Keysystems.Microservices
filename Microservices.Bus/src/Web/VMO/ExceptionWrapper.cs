using System;

namespace Microservices.Bus.Web.VMO
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ExceptionWrapper
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public ExceptionWrapper()
		{ }
		#endregion


		#region Properties
		///// <summary>
		///// {Get}
		///// </summary>
		//public string ErrorType { get; private set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public DateTime? Time { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public string InnerMessages { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public string FullMessage { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public string Source { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public string StackTrace { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public ExceptionWrapper InnerException { get; set; }
		#endregion

	}
}
