using System;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Channels;

namespace Microservices.Bus.Channels
{
	public interface IChannelContext : IDisposable
	{
		ChannelInfo Info { get; }

		ChannelStatus Status { get; }

		IChannel Channel { get; }

		IMicroserviceClient Client { get; }

		/// <summary>
		/// {Get,Set} Ошибка.
		/// </summary>
		Exception LastError { get; set; }


		Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken = default);

		Task TerminateChannelAsync(CancellationToken cancellationToken = default);

	}
}
