using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

using Microservices.Channels.Data;

namespace Microservices.Channels.MSSQL.Data
{
	/// <summary>
	/// 
	/// </summary>
	public class MessageContentStream : MessageContentStreamBase
	{
		private readonly string tableName = Database.Tables.MESSAGE_CONTENTS;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="work"></param>
		/// <param name="mode"></param>
		/// <param name="contentLink"></param>
		/// <param name="encoding"></param>
		public MessageContentStream(DbContext dbContext, UnitOfWork work, DataStreamMode mode, int contentLink, Encoding encoding)
			: base(dbContext, work, mode, contentLink, encoding)
		{
			switch ( mode )
			{
				case DataStreamMode.READ:
					{
						string sql = String.Format("SELECT VALUE FROM {0} WHERE LINK={1}", this.tableName, this.ContentLINK);
						this.command = new SqlCommand(sql, (SqlConnection)work.Session.Connection);
						this.command.CommandTimeout = this.ReadTimeout;
					}
					break;
				case DataStreamMode.WRITE:
					{
						string sql = String.Format("UPDATE {0} SET VALUE = ISNULL(VALUE, '') WHERE LINK={1};", this.tableName, this.ContentLINK);
						sql += String.Format("UPDATE {0} SET VALUE .WRITE(@data, null, null) WHERE LINK={1};", this.tableName, this.ContentLINK);
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
				string sql = String.Format("SELECT LEN(VALUE) FROM {0} WHERE LINK={1}", this.tableName, this.ContentLINK);
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
