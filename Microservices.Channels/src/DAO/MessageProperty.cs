using System;
using System.Diagnostics;

namespace Microservices.Channels.DAO
{
	/// <summary>
	/// Дополнительное свойство сообщения.
	/// </summary>
	public class MessageProperty : MarshalByRefObject
	{

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public virtual int LINK { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		///
		/// </summary>
		public virtual string Value { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string Type { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string Format { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual string Comment { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual Message Message { get; set; }
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
