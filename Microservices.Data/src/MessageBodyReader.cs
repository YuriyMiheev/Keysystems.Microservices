using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microservices.Data
{
	/// <summary>
	/// 
	/// </summary>
	public class MessageBodyReader : TextReader
	{
		private int bufferSize;
		private MessageBodyStreamBase baseStream;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="bufferSize">Больше или равно 1024.</param>
		public MessageBodyReader(MessageBodyStreamBase stream, int bufferSize)
		{
			#region Validate parameters
			if ( stream == null )
				throw new ArgumentNullException("stream");

			if ( !stream.CanRead )
				throw new ArgumentException("Поток не доступен для чтения.", "stream");

			if ( bufferSize < 1024 )
				throw new ArgumentException("Недостаточный размер буфера данных.", "bufferSize");
			#endregion

			this.baseStream = stream;
			this.bufferSize = bufferSize;
		}
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int Peek()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int Read()
		{
			return this.baseStream.Read();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public override int Read(char[] buffer, int index, int count)
		{
			return this.baseStream.Read(buffer, index, count);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ReadLine()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ReadToEnd()
		{
			return this.baseStream.ReadToEnd();
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Close()
		{
			this.baseStream.Close();
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
				this.baseStream.Dispose();
			}

			disposed = true;
		}
		#endregion

	}
}
