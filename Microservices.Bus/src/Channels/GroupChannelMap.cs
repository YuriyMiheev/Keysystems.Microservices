using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Связь канал/группа.
	/// </summary>
	public class GroupChannelMap
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public GroupChannelMap()
		{ }
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public virtual int LINK { get; set; }

		/// <summary>
		/// {Get,Set} Приложение.
		/// </summary>
		public virtual int? GroupLINK { get; set; }

		/// <summary>
		/// {Get} Канал.
		/// </summary>
		public virtual int? ChannelLINK { get; set; }
		#endregion

	}
}
