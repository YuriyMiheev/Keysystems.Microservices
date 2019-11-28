using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Channels.MSSQL
{
	public interface IMessageReceiver
	{
		Message ReceiveMessage(Message msg);
	}
}
