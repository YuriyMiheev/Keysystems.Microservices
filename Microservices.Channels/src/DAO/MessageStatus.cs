using System;

namespace Microservices.Channels.DAO
{
	/// <summary>
	/// Статусы сообщений и посылок.
	/// </summary>
	public class MessageStatus : MarshalByRefObject
	{

		#region Properties
		/// <summary>
		/// {Get,Set}
		/// </summary>
		public virtual string Value { get; set; }

		/// <summary>
		/// {Get,Set} Дата и время.
		/// </summary>
		public virtual DateTime? Date { get; set; }

		/// <summary>
		/// {Get,Set} Информация.
		/// </summary>
		public virtual string Info { get; set; }

		/// <summary>
		/// {Get,Set} Код статуса.
		/// </summary>
		public virtual int? Code { get; set; }
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
