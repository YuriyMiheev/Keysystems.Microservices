using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Bus.Logging
{
	public class BusLogger : ILogger
	{
		public void InitializeLogger()
		{
		}

		public void LogError(Exception error)
		{
		}

		public void LogError(string text, Exception error)
		{
		}

		public void LogInfo(string text)
		{
		}

		public void LogTrace(string text)
		{
		}
	}
}
