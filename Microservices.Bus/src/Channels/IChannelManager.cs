using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Bus.Channels
{
	public interface IChannelManager
	{
		/// <summary>
		/// Группы каналов.
		/// </summary>
		GroupInfo[] ChannelsGroups { get; }

		/// <summary>
		/// Активные каналы.
		/// </summary>
		IChannelContext[] RuntimeChannels { get; }


		/// <summary>
		/// Загрузить каналы по списку из БД.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task LoadChannelsAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Прервать работу канала.
		/// </summary>
		/// <param name="virtAddress"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task TerminateChannelAsync(string virtAddress, CancellationToken cancellationToken = default);
	}
}
