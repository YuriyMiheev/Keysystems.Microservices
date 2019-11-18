using System;

namespace Microservices.Channels
{
	/// <summary>
	/// Направление передачи сообщения.
	/// </summary>
	public class MessageDirection
	{
		/// <summary>
		/// Входящее сообщение.
		/// </summary>
		public const string IN = "IN";

		/// <summary>
		/// Исходящее сообщение.
		/// </summary>
		public const string OUT = "OUT";
	}
}
