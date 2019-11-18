using System;

namespace Microservices.Channels.Data
{
	partial class Database
	{
		/// <summary>
		/// Хранимые процедуры.
		/// </summary>
		public class StoredProcedure
		{
			/// <summary>
			/// "rms_Ping()"
			/// </summary>
			public const string PING = "rms_Ping";

			/// <summary>
			/// "rms_ReceiveMessage(int msgLink)"
			/// </summary>
			public const string ReceiveMessage = "rms_ReceiveMessage";

			/// <summary>
			/// "rms_MessageStatusChanged(int msgLink, string prevStatus, string newStatus)"
			/// </summary>
			public const string MessageStatusChanged = "rms_MessageStatusChanged";
		}
	}
}
