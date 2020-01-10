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
		/// Шаблон нового канала.
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		ChannelInfo NewChannelTemplate(string provider);

		IChannelContext GetChannel(int channelLink);

		IChannelContext GetChannel(string virtAddress);

		IChannelContext FindChannel(int channelLink);

		IChannelContext FindChannel(string virtAddress);

		/// <summary>
		/// Стартовать (активировать и открыть) канал.
		/// </summary>
		/// <param name="channelLink"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task StartChannelAsync(int channelLink, CancellationToken cancellationToken = default);

		/// <summary>
		/// Стартовать (активировать и открыть) канал.
		/// </summary>
		/// <param name="virtAddress"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task StartChannelAsync(string virtAddress, CancellationToken cancellationToken = default);

		/// <summary>
		/// Прервать работу канала.
		/// </summary>
		/// <param name="virtAddress"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task TerminateChannelAsync(string virtAddress, CancellationToken cancellationToken = default);
	}
}
