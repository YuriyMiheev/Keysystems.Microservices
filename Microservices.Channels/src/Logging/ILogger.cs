﻿using System;

namespace Microservices.Channels.Logging
{
	/// <summary>
	/// Методы логирования.
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// 
		/// </summary>
		void InitializeLogger();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="error"></param>
		void LogError(Exception error);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="error"></param>
		void LogError(string text, Exception error);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		void LogInfo(string text);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		void LogTrace(string text);
	}
}
