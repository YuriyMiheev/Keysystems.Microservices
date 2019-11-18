using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservices.Channels
{
	/// <summary>
	/// Версия сообщения.
	/// </summary>
	public class MessageVersion
	{
		/// <summary>
		/// "1.0"
		/// </summary>
		public const string V1_0 = "1.0";

		/// <summary>
		/// "1.1"
		/// </summary>
		/// <remarks>
		/// Изменения:
		/// 1. В статус добавлен код статуса;
		/// 2. В посылку добавлен счетчик попыток доставки.
		/// 3. В сообщение добавлено имя очереди.
		/// </remarks>
		public const string V1_1 = "1.1";

		/// <summary>
		/// "1.2"
		/// </summary>
		/// <remarks>
		/// Изменения:
		/// 1. У сообщения убрано имя очереди. Очередь формируется по адресам получателей.
		/// </remarks>
		public const string V1_2 = "1.2";


		/// <summary>
		/// {Get} Текущая версия.
		/// </summary>
		public static string Current
		{
			get { return MessageVersion.V1_2; }
		}
	}
}
