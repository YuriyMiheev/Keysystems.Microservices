﻿using System;

namespace Microservices.Channels.DAO
{
	/// <summary>
	/// Содержимое сообщения.
	/// </summary>
	public class MessageContentInfo : MarshalByRefObject
	{

		#region Properties
		/// <summary>
		/// {Get} 
		/// </summary>
		public virtual int LINK { get;  set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual Message Message { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Type { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual int? Length { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual int? FileSize { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string Comment { get; set; }
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
