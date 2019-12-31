using System;

using Microservices.Logging;

namespace Microservices.Channels.Logging
{
	public class ChannelLogger : ILogger
	{
		private readonly IConsoleLogger _consoleLogger;


		public ChannelLogger(IConsoleLogger consoleLogger)
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
