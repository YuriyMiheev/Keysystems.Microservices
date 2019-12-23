using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservices
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ConnectionChangedEventArgs : EventArgs
	{
		/// <summary>
		/// {Get,Set}
		/// </summary>
		public bool ConnectionExists { get; set; }
	}
}
