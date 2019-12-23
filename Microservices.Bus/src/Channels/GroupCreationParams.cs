using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	[DebuggerDisplay("{this.Name}")]
	public sealed class GroupCreationParams
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public GroupCreationParams()
		{ }
		#endregion


		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Image { get; set; }
		#endregion

	}
}
