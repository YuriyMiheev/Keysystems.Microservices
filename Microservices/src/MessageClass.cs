using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservices
{
	/// <summary>
	/// Класс сообщения.
	/// </summary>
	public class MessageClass
	{
		/// <summary>
		/// "REQUEST" - Запрос.
		/// </summary>
		public const string REQUEST = "REQUEST";

		/// <summary>
		/// "RESPONSE" - Ответ.
		/// </summary>
		public const string RESPONSE = "RESPONSE";

		/// <summary>
		/// "PUBLISH" - Публикация.
		/// </summary>
		public const string PUBLISH = "PUBLISH";
	}
}
