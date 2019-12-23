using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Microservices.Data
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class MessageBodyStreamBase : DataStreamBase
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="work"></param>
		/// <param name="mode"></param>
		/// <param name="msgLink"></param>
		/// <param name="encoding"></param>
		protected MessageBodyStreamBase(DbContext dbContext, UnitOfWork work, DataStreamMode mode, int msgLink, Encoding encoding)
			: base(dbContext, work, mode, encoding)
		{
			this.MessageLINK = msgLink;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public int MessageLINK { get; private set; }
		#endregion

	}
}
