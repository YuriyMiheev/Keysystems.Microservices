using System;

namespace Microservices.Channels
{
	public interface IChannelControl
	{
		void OpenChannel();

		void CloseChannel();

		void RunChannel();

		void StopChannel();


		#region Diagnostic
		bool TryConnect(out Exception error);

		void CheckState();

		void Repair();

		void Ping();
		#endregion


		#region Error
		/// <summary>
		/// Сбросить ошибку.
		/// </summary>
		void ClearError();

		/// <summary>
		/// Запомнить ошибку.
		/// </summary>
		/// <param name="error"></param>
		void SetError(Exception error);

		///// <summary>
		///// Вызвать ошибку.
		///// </summary>
		///// <param name="text"></param>
		///// <returns></returns>
		//ChannelException ThrowError(string text);
		#endregion

	}
}
