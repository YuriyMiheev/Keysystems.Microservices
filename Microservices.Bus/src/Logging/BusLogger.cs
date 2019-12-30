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
			Console.WriteLine($"[ERROR] [{DateTime.Now}] {error}");
		}

		public void LogError(string text, Exception error)
		{
			Console.WriteLine($"[ERROR] [{DateTime.Now}] {text} {Environment.NewLine} {error}");
		}

		public void LogInfo(string text)
		{
			Console.WriteLine($"[INFO]  [{DateTime.Now}] {text}");
		}

		public void LogTrace(string text)
		{
			Console.WriteLine($"[TRACE] [{DateTime.Now}] {text}");
		}
	}
}
