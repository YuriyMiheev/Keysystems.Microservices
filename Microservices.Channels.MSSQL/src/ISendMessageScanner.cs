using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Channels.MSSQL
{
	public interface ISendMessageScanner
	{

		#region Events
		event Func<Message[], bool> NewMessages;
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		string Recipient { get; }
		#endregion


		void Start(string recipient, CancellationToken cancellationToken = default);

	}
}
