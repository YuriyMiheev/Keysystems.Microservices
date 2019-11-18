using System;

namespace Microservices.Channels.DAO
{
	/// <summary>
	/// Тело сообщения.
	/// </summary>
	public class MessageBody : MarshalByRefObject
	{

		#region Properties
		/// <summary>
		/// {Get} ID сообщения.
		/// </summary>
		public virtual int MessageLINK { get; set; }

		/// <summary>
		/// {Get,Set} Имя.
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// {Get,Set} Тип.
		/// </summary>
		public virtual string Type { get; set; }

		/// <summary>
		/// {Get,Set} Длина.
		/// </summary>
		public virtual int? Length { get; set; }

		/// <summary>
		/// {Get,Set} Размер.
		/// </summary>
		public virtual int? FileSize { get; set; }

		/// <summary>
		/// {Get,Set} Значение.
		/// </summary>
		public virtual string Value { get; set; }
		#endregion


		#region MarshalByRefObject
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override object InitializeLifetimeService()
		{
			return null;
		}
		#endregion

	}
}
