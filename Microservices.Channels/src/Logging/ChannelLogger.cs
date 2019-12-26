using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Channels.Logging
{
	public class ChannelLogger : ILogger
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
