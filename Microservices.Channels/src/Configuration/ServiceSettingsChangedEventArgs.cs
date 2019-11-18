using System;

namespace Microservices.Common.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ServiceSettingsChangedEventArgs : EventArgs
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public ServiceSettingsChangedEventArgs()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public ServiceSettingsChangedEventArgs(string message)
		{
			if (String.IsNullOrEmpty(message))
				throw new ArgumentException("message");

			this.Message = message;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get,Set}
		/// </summary>
		public ServiceSettingsChangeType ChangeType { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public string Message { get; set; }
		#endregion


		#region Metrhods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Message;
		}
		#endregion

	}
}
