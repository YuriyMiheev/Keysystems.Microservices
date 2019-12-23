using System;

namespace Microservices
{
	/// <summary>
	/// Обертка над ошибкой.
	/// </summary>
	[Serializable]
	public class ExceptionWrapper
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		private ExceptionWrapper()
		{ }

		/// <summary>
		/// 
		/// </summary>
		public ExceptionWrapper(Exception error)
		{
			#region Validate parameters
			if ( error == null )
				throw new ArgumentNullException("error");
			#endregion

			this.ErrorType = error.GetType().Name;
			this.Time = DateTime.Now;
			this.Message = error.Message;
			string allMessages = error.AllMessages();
			if ( !String.IsNullOrEmpty(allMessages) )
				this.InnerMessages = allMessages.Replace(this.Message, "").Trim('\r', '\n');

			this.FullMessage = error.ToString();
			this.Source = error.Source;
			this.StackTrace = error.StackTrace;
			if ( error.InnerException != null )
				this.InnerException = new ExceptionWrapper(error.InnerException);
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public string ErrorType { get; private set; }

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
