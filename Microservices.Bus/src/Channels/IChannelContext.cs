using System;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Channels;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Контекст канала.
	/// </summary>
	public interface IChannelContext : IDisposable
	{
		/// <summary>
		/// Информация о канале.
		/// </summary>
		ChannelInfo Info { get; }

		/// <summary>
		/// Статус канала.
		/// </summary>
		ChannelStatus Status { get; }

		/// <summary>
		/// Инстанс канала.
		/// </summary>
		IChannel Channel { get; }

		/// <summary>
		/// Клиент канала.
		/// </summary>
		IChannelClient Client { get; }

		/// <summary>
		/// Последняя зафиксированная ошибка.
		/// </summary>
		Exception LastError { get; set; }


		/// <summary>
		/// Активировать канал.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task ActivateChannelAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Разрушить канал.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task TerminateChannelAsync(CancellationToken cancellationToken = default);

	}
}
