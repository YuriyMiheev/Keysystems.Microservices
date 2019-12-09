using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Microservices.Channels
{
	/// <summary>
	/// Ошибка канала.
	/// </summary>
	[Serializable]
	public class ChannelException : Exception
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		protected ChannelException()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="message"></param>
		public ChannelException(IChannelService channel, string message)
			: base(FormatMessage(channel, message))
		{
			this.Channel = channel.VirtAddress;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public ChannelException(IChannelService channel, string message, Exception inner)
			: base(FormatMessage(channel, message), inner)
		{
			this.Channel = channel.VirtAddress;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ChannelException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.Channel = info.GetString("ChannelLink");
			this.ErrorCode = info.GetString("ErrorCode");
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public string Channel { get; private set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public string ErrorCode { get; set; }
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("ChannelLink", this.Channel);
			info.AddValue("ErrorCode", this.ErrorCode);
		}
		#endregion


		#region Helpers
		private static string FormatMessage(IChannelService channel, string message)
		{
			return message;
		}
		#endregion


		#region Error Codes
		/// <summary>
		/// 
		/// </summary>
		public const string DatabaseSchemaInvalid = "DatabaseSchemaInvalid";

		/// <summary>
		/// 
		/// </summary>
		public const string NoMsgboxConnection = "NoMsgboxConnection";

		/// <summary>
		/// 
		/// </summary>
		public const string NoRealConnection = "NoRealConnection";

		/// <summary>
		/// 
		/// </summary>
		public const string DatabaseUsedOtherRms = "DatabaseUsedOtherRms";

		/// <summary>
		/// 
		/// </summary>
		public const string DatabaseUsedOtherChannel = "DatabaseUsedOtherChannel";
		#endregion

	}
}
