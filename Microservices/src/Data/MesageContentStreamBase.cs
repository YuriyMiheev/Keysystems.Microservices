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
	public abstract class MessageContentStreamBase : DataStreamBase
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="work"></param>
		/// <param name="mode"></param>
		/// <param name="contentLink"></param>
		/// <param name="encoding"></param>
		protected MessageContentStreamBase(DbContext dbContext, UnitOfWork work, DataStreamMode mode, int contentLink, Encoding encoding)
			: base(dbContext, work, mode, encoding)
		{
			this.ContentLINK = contentLink;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public int ContentLINK { get; private set; }
		#endregion

	}
}
