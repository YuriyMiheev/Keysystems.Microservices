using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Bus.Channels
{
	public interface IChannelContext : IDisposable
	{
		ChannelInfo ChannelInfo { get; }

		bool IsChannelCreated { get; }

		IChannel Channel { get ; }

		/// <summary>
		/// {Get,Set} Ошибка.
		/// </summary>
		Exception LastError { get; set; }


		Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken = default);

		Task TerminateChannelAsync(CancellationToken cancellationToken = default);

	}
}
