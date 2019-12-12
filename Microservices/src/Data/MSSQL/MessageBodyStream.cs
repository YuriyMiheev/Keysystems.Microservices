using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

using Microservices.Data;

namespace Microservices.Data.MSSQL
{
	/// <summary>
	/// 
	/// </summary>
	public class MessageBodyStream : MessageBodyStreamBase
	{
		private readonly string tableName = Database.Tables.MESSAGES;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="work"></param>
		/// <param name="mode"></param>
		/// <param name="msgLink"></param>
		/// <param name="encoding"></param>
		public MessageBodyStream(DbContext dbContext, UnitOfWork work, DataStreamMode mode, int msgLink, Encoding encoding)
			: base(dbContext, work, mode, msgLink, encoding)
		{
			switch ( mode )
			{
				case DataStreamMode.READ:
					{
						string sql = String.Format("SELECT BODY_VALUE FROM {0} WHERE LINK={1}", this.tableName, this.MessageLINK);
						this.command = new SqlCommand(sql, (SqlConnection)work.Session.Connection);
						this.command.CommandTimeout = this.ReadTimeout;
					}
					break;
				case DataStreamMode.WRITE:
					{
						string sql = String.Format("UPDATE {0} SET BODY_VALUE = ISNULL(BODY_VALUE, '') WHERE LINK={1};", this.tableName, this.MessageLINK);
						sql += String.Format("UPDATE {0} SET BODY_VALUE .WRITE(@data, null, null) WHERE LINK={1};", this.tableName, this.MessageLINK);
						this.command = new SqlCommand(sql, (SqlConnection)work.Session.Connection);
						this.parameter = new SqlParameter("@data", SqlDbType.NVarChar);
						this.command.CommandTimeout = this.WriteTimeout;
						this.command.Parameters.Add(this.parameter);
					}
					break;
			}

			if ( this.Work.Transaction != null )
				this.Work.Transaction.Enlist(this.command);
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public override long Length
		{
			get
			{
				string sql = String.Format("SELECT LEN(BODY_VALUE) FROM {0} WHERE LINK={1}", this.tableName, this.MessageLINK);
				var cmd = new SqlCommand(sql, (SqlConnection)this.Work.Session.Connection);

				if ( this.Work.Transaction != null )
					this.Work.Transaction.Enlist(cmd);

				object result = cmd.ExecuteScalar();
				if ( result is DBNull )
					return 0;
				else
					return Convert.ToInt64(result);
			}
		}
		#endregion

	}
}
