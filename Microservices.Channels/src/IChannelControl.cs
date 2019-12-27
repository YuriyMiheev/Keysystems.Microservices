using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Channels
{
	public interface IChannelControl
	{
		Task OpenChannelAsync(CancellationToken cancellationToken = default);

		Task CloseChannelAsync(CancellationToken cancellationToken = default);

		Task RunChannelAsync(CancellationToken cancellationToken = default);

		Task StopChannelAsync(CancellationToken cancellationToken = default);


		#region Diagnostic
		Task<bool> TryConnectAsync(out Exception error);

		void CheckState();

		void Repair();

		void Ping();
		#endregion


		//#region Error
		///// <summary>
		///// Сбросить ошибку.
		///// </summary>
		//void ClearError();

		///// <summary>
		///// Запомнить ошибку.
		///// </summary>
		///// <param name="error"></param>
		//void SetError(Exception error);

		///// <summary>
		///// Вызвать ошибку.
		///// </summary>
		///// <param name="text"></param>
		///// <returns></returns>
		//ChannelException ThrowError(string text);
		//#endregion

	}
}
