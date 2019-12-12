// http://www.codeproject.com/Articles/140713/Download-and-Upload-Images-from-SQL-Server-via-ASP
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;

namespace Microservices.Data
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class DataStreamBase : Stream
	{

		#region Fields
		/// <summary>
		/// 
		/// </summary>
		protected DbDataReader valueReader;

		/// <summary>
		/// 
		/// </summary>
		protected DbCommand command;

		/// <summary>
		/// 
		/// </summary>
		protected DbParameter parameter;
		#endregion


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="work"></param>
		/// <param name="mode"></param>
		/// <param name="encoding"></param>
		protected DataStreamBase(DbContext dbContext, UnitOfWork work, DataStreamMode mode, Encoding encoding)
		{
			#region Validate parameters
			if ( dbContext == null )
				throw new ArgumentNullException("dbContext");

			if ( work == null )
				throw new ArgumentNullException("work");

			if ( encoding == null )
				throw new ArgumentNullException("encoding");
			#endregion

			this.DbContext = dbContext;
			this.Work = work;
			this.Mode = mode;
			this.Encoding = encoding;

			this.ReadTimeout = this.WriteTimeout = 30;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public DbContext DbContext { get; private set; }

		/// <summary>
		/// {Get}
		/// </summary>
		public UnitOfWork Work { get; private set; }

		/// <summary>
		/// {Get}
		/// </summary>
		public DataStreamMode Mode { get; private set; }

		/// <summary>
		/// {Get}
		/// </summary>
		public virtual Encoding Encoding { get; private set; }

		/// <summary>
		/// {Get}
		/// </summary>
		public override bool CanRead
		{
			get { return (this.Mode == DataStreamMode.READ); }
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public override bool CanSeek
		{
			get { return true; }
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public override bool CanWrite
		{
			get { return (this.Mode == DataStreamMode.WRITE); }
		}

		protected long? length;
		/// <summary>
		/// 
		/// </summary>
		public override long Length
		{
			get { return (length.HasValue ? length.Value : 0); }
		}

		private long position;
		/// <summary>
		/// {Get,Set}
		/// </summary>
		public override long Position
		{
			get { return position; }
			set { position = value; }
		}

		/// <summary>
		/// {Get,Set} Таймаут на чтение (сек).
		/// </summary>
		public override int ReadTimeout { get; set; }

		/// <summary>
		/// {Get,Set} Таймаут на запись (сек).
		/// </summary>
		public override int WriteTimeout { get; set; }
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		public override void Flush()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual int Read()
		{
			char[] buffer = new char[1];
			int charsReaded = Read(buffer, 0, buffer.Length);
			if ( charsReaded == 0 )
				return 0;

			return buffer[0];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public virtual int Read(char[] buffer, int offset, int count)
		{
			if ( this.valueReader == null )
			{
				this.length = this.Length;
				if ( !InitReader() )
					return 0;
			}

			if ( this.length == 0 )
				return 0;

			int size = (int)(this.length - this.Position);
			if ( size == 0 )
				return 0;

			if ( size > buffer.Length )
				size = buffer.Length;

			int charsReaded = (int)this.valueReader.GetChars(0, this.Position, buffer, offset, size);
			this.Position += charsReaded;

			return charsReaded;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual string ReadToEnd()
		{
			char[] buffer = new char[this.Length];
			int charsReaded = Read(buffer, 0, buffer.Length);
			if ( charsReaded == 0 )
				return null;

			return new String(buffer, 0, charsReaded);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			char[] chars = new char[count];
			int charsReaded = Read(chars, 0, chars.Length);
			if ( charsReaded == 0 )
				return 0;

			int bytesReaded = this.Encoding.GetBytes(chars, 0, charsReaded, buffer, offset);
			this.Position += bytesReaded;
			return bytesReaded;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="offset"></param>
		/// <param name="origin"></param>
		/// <returns></returns>
		public override long Seek(long offset, SeekOrigin origin)
		{
			switch ( origin )
			{
				case SeekOrigin.Begin:
					this.Position -= offset;
					break;
				case SeekOrigin.End:
					this.Position += offset;
					break;
			}

			return this.Position;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public override void SetLength(long value)
		{
			this.length = value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		public virtual void Write(char[] buffer, int offset, int count)
		{
			if ( this.Mode == DataStreamMode.READ )
				throw new InvalidOperationException(String.Format("Невозможно выполнить запись в режиме {0}.", this.Mode));

			string value = new String(buffer, offset, count);
			this.parameter.Value = value;
			this.command.ExecuteNonQuery();

			this.Position += value.Length;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		public override void Write(byte[] buffer, int offset, int count)
		{
			string s = this.Encoding.GetString(buffer, offset, count);
			Write(s.ToCharArray(), 0, s.Length);
		}
		#endregion


		#region Helpers
		private bool InitReader()
		{
			if ( this.valueReader == null )
			{
				if ( this.Mode == DataStreamMode.WRITE )
					throw new InvalidOperationException(String.Format("Невозможно выполнить чтение в режиме {0}.", this.Mode));

				// Попытка предотвратить System.OutOfMemoryException
				if ( this.DbContext.Provider == "MSSQL" )
					this.valueReader = this.command.ExecuteReader(CommandBehavior.SequentialAccess);
				else
					this.valueReader = this.command.ExecuteReader();

				if ( !this.valueReader.Read() )
					return false;
			}

			return true;
		}
		#endregion


		#region IDisposable
		private bool disposed;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if ( disposed )
				return;

			base.Dispose(disposing);

			if ( disposing )
			{
				if ( this.valueReader != null )
					this.valueReader.Dispose();

				if ( this.command != null )
					this.command.Dispose();

				this.Work.Dispose();
			}

			disposed = true;
		}
		#endregion

	}
}
