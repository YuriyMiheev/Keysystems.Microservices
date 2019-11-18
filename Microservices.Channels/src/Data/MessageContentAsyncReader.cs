using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microservices.Channels.Data
{
	/// <summary>
	/// 
	/// </summary>
	public class MessageContentAsyncReader : TextReader
	{
		private IAsyncEnumerable<char[]> _baseStream;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		public MessageContentAsyncReader(IAsyncEnumerable<char[]> contentStream)
		{
			_baseStream = contentStream ?? throw new ArgumentNullException("contentStream");
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
		/// <param name="buffer"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public override int Read(char[] buffer, int index, int count)
		{
			var enumerator = _baseStream.GetAsyncEnumerator();
			if (enumerator.MoveNextAsync().Result)
			{
				char[] chars = enumerator.Current; //.Take(count).ToArray();
				chars.CopyTo(buffer, index);
				return chars.Count();
			}

			return 0;
		}
		#endregion

	}
}
