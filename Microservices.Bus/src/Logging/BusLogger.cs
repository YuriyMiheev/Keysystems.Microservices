using System;

using Microservices.Logging;

namespace Microservices.Bus.Logging
{
	public class BusLogger : ILogger
	{
		private readonly IConsoleLogger _consoleLogger;


		public BusLogger(IConsoleLogger consoleLogger)
		{
			_consoleLogger = consoleLogger ?? throw new ArgumentNullException(nameof(consoleLogger));
		}


		public void InitializeLogger()
		{
			_consoleLogger.InitializeLogger();
		}

		public void LogError(Exception error)
		{
			_consoleLogger.LogError(error);
		}

		public void LogError(string text, Exception error)
		{
			_consoleLogger.LogError(text, error);
		}

		public void LogInfo(string text)
		{
			_consoleLogger.LogInfo(text);
		}

		public void LogTrace(string text)
		{
			_consoleLogger.LogTrace(text);
		}
	}
}
