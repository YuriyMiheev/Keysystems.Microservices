using System;

namespace Microservices.Data
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
			public const string PingSP = "rms_Ping";

			/// <summary>
			/// "rms_ReceiveMessage(int msgLink)"
			/// </summary>
			public const string ReceiveMessageSP = "rms_ReceiveMessage";

			/// <summary>
			/// "rms_MessageStatusChanged(int msgLink, string prevStatus, string newStatus)"
			/// </summary>
			public const string MessageStatusChangedSP = "rms_MessageStatusChanged";

			/// <summary>
			/// "rms_Repair()"
			/// </summary>
			public const string RepairSP = "rms_Repair";
		}
	}
}
