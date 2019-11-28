using System;
using System.Threading;

namespace Microservices.Channels.MSSQL
{
	public interface ISendMessageScanner
	{

		event Func<Message[], bool> NewMessages;

		event Action<Exception> Error;

		///// <summary>
		///// {Get}
		///// </summary>
		//string Recipient { get; }


		void Start(TimeSpan interval, int portion, CancellationToken cancellationToken = default);

	}
}
