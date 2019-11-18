using System;
using System.Data.Common;
using System.Runtime.Serialization;

namespace Microservices.Channels.Data
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class StoredProcedureException : DbException
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public StoredProcedureException()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public StoredProcedureException(string message)
			: base(message)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public StoredProcedureException(string message, Exception inner)
			: base(message, inner)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected StoredProcedureException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion

	}
}
