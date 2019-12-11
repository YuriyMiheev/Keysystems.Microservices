using System;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Канал сервиса.
	/// </summary>
	public interface IChannel : IChannelRuntime
	{

		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		bool Enabled { get; }

		/// <summary>
		/// {Get}
		/// </summary>
		IChannelRuntime Runtime { get; }

		/// <summary>
		/// {Get}
		/// </summary>
		bool IsRuntimeCreated { get; }
		#endregion


		#region Methods

		#region Info
		/// <summary>
		/// Обновить информацию о канале.
		/// </summary>
		/// <param name="updateParams"></param>
		/// <param name="save"></param>
		void UpdateInfo(ChannelInfoUpdateParams updateParams, bool save = false);
		
		/// <summary>
		/// Сохранить информацию о канале.
		/// </summary>
		/// <returns></returns>
		void SaveInfo();
		#endregion


		#region Control
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IChannelRuntime CreateRuntime();

		/// <summary>
		/// Открыть канал.
		/// </summary>
		/// <param name="timeout"></param>
		void Open(TimeSpan timeout);

		/// <summary>
		/// Закрыть канал.
		/// </summary>
		/// <param name="timeout"></param>
		void Close(TimeSpan timeout);

		/// <summary>
		/// Запустить канал.
		/// </summary>
		/// <param name="timeout"></param>
		void Run(TimeSpan timeout);

		/// <summary>
		/// Остановить канал.
		/// </summary>
		/// <param name="timeout"></param>
		void Stop(TimeSpan timeout);
		#endregion

		#endregion

	}
}
