using System;

namespace Microservices
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
