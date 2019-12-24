using System;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Data;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Канал шины.
	/// </summary>
	public interface IChannel : IMessageRepository, IDisposable
	{

		//#region Info
		///// <summary>
		///// Обновить информацию о канале.
		///// </summary>
		///// <param name="updateParams"></param>
		///// <param name="save"></param>
		//void UpdateInfo(ChannelInfoUpdateParams updateParams, bool save = false);

		///// <summary>
		///// Сохранить информацию о канале.
		///// </summary>
		///// <returns></returns>
		//void SaveInfo();
		//#endregion


		#region Control
		/// <summary>
		/// Открыть канал.
		/// </summary>
		Task OpenAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Закрыть канал.
		/// </summary>
		Task CloseAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Запустить канал.
		/// </summary>
		Task RunAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Остановить канал.
		/// </summary>
		Task StopAsync(CancellationToken cancellationToken = default);
		#endregion


		#region Diagnostic
		/// <summary>
		/// Проверить подключение к каналу.
		/// </summary>
		/// <param name="error"></param>
		/// <returns></returns>
		bool TryConnect(out Exception error);

		/// <summary>
		/// Проверить состояние канала.
		/// </summary>
		void CheckState();

		/// <summary>
		/// Попытаться восстановить работоспособность канала.
		/// </summary>
		void Repair();

		/// <summary>
		/// Ping канала.
		/// </summary>
		void Ping();
		#endregion

	}
}
