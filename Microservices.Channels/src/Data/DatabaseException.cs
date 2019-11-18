using System;
using System.Data;
using System.Data.Common;
using System.Runtime.Serialization;

namespace Microservices.Channels.Data
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class DatabaseException : DbException
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public DatabaseException()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public DatabaseException(string message)
			: base(message)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public DatabaseException(string message, Exception inner)
			: base(message, inner)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected DatabaseException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion

	}
}
