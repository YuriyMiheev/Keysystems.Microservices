using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Logging
{
	public class ConsoleLogger : IConsoleLogger
	{
		public void InitializeLogger()
		{
		}

		public void LogError(Exception error)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("[ERROR]");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine($" [{DateTime.Now}] {error}");
		}

		public void LogError(string text, Exception error)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("[ERROR]");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine($" [{DateTime.Now}] {text} {Environment.NewLine} {error}");
		}

		public void LogInfo(string text)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("[INFO]");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine($"  [{DateTime.Now}] {text}");
		}

		public void LogTrace(string text)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("[TRACE]");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine($" [{DateTime.Now}] {text}");
		}
	}
}
