using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservices.Channels
{
	/// <summary>
	/// Тип сообщения.
	/// </summary>
	public class MessageType
	{
		/// <summary>
		/// Документ.
		/// </summary>
		public const string DOCUMENT = "DOCUMENT";

		///// <summary>
		///// Почтовое сообщение.
		///// </summary>
		//public const string MAIL = "MAIL";

		///// <summary>
		///// Электронное письмо.
		///// </summary>
		//public const string EMAIL = "EMAIL";

		/// <summary>
		/// Команда.
		/// </summary>
		public const string COMMAND = "COMMAND";

		/// <summary>
		/// Событие.
		/// </summary>
		public const string EVENT = "EVENT";
	}
}
