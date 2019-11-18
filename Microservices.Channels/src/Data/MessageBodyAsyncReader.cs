using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Channels.Data
{
	/// <summary>
	/// 
	/// </summary>
	public class MessageBodyAsyncReader : TextReader
	{
		private IAsyncEnumerable<char[]> baseStream;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		public MessageBodyAsyncReader(IAsyncEnumerable<char[]> stream)
		{
			#region Validate parameters
			if (stream == null)
				throw new ArgumentNullException("stream");
			#endregion

			this.baseStream = stream;
		}
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public override int Read(char[] buffer, int index, int count)
		{
			var enumerator = this.baseStream.GetAsyncEnumerator();
			if (enumerator.MoveNextAsync().Result)
			{
				char[] chars = enumerator.Current;
				chars.CopyTo(buffer, index);
				return chars.Count();
			}

			return 0;
		}

		public async override Task<int> ReadAsync(char[] buffer, int index, int count)
		{
			var enumerator = this.baseStream.GetAsyncEnumerator();
			if (await enumerator.MoveNextAsync())
			{
				char[] chars = enumerator.Current;
				chars.CopyTo(buffer, index);
				return chars.Count();
			}

			return 0;
		}
		#endregion

	}
}
