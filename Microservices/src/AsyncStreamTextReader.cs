using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Microservices
{
	/// <summary>
	/// 
	/// </summary>
	public class AsyncStreamTextReader : TextReader
	{
		private IAsyncEnumerator<char[]> _asyncEnumerator;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream"></param>
		public AsyncStreamTextReader(IAsyncEnumerable<char[]> stream)
		{
			_asyncEnumerator = (stream ?? throw new ArgumentNullException(nameof(stream))).GetAsyncEnumerator();
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
			return ReadAsync(buffer, index, count).Result;
		}

		public async override Task<int> ReadAsync(char[] buffer, int index, int count)
		{
			if (await _asyncEnumerator.MoveNextAsync())
			{
				_asyncEnumerator.Current.CopyTo(buffer, index);
				return _asyncEnumerator.Current.Length;
			}

			return 0;
		}

		public async override Task<string> ReadToEndAsync()
		{
			var text = new StringBuilder();

			if (await _asyncEnumerator.MoveNextAsync())
				text.Append(_asyncEnumerator.Current);

			return text.ToString();
		}
		#endregion

	}
}
