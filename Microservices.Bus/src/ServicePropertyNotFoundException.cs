using System;
using System.Runtime.Serialization;

namespace Microservices.Bus
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ServicePropertyNotFoundException : Exception
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public ServicePropertyNotFoundException()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public ServicePropertyNotFoundException(string message)
			: base(message)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public ServicePropertyNotFoundException(string message, Exception inner)
			: base(message, inner)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ServicePropertyNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion

	}
}
