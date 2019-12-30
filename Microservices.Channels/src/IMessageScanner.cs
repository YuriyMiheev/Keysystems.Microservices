using System;
using System.Threading;

namespace Microservices.Channels
{
	public interface IMessageScanner
	{

		event Func<Message[], bool> NewMessages;


		void StartScan(TimeSpan interval, int portion, CancellationToken cancellationToken = default);

		void StopScan();

	}
}
