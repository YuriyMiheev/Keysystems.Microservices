using System;
using System.Collections.Generic;
using System.Data;

namespace Microservices.Data
{
	/// <summary>
	/// Адаптер хранилища сообщений.
	/// </summary>
	public interface IMessageDataAdapter : IMessageRepository, IDataAdapter
	{
	}
}
