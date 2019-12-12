using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservices.Bus
{
	/// <summary>
	/// 
	/// </summary>
	public class GroupInfo
	{
		/// <summary>
		/// "Default"
		/// </summary>
		public static string DefaultGroupName = "Default";


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public GroupInfo()
		{
			this.Channels = new int[0];
			this.Image = "ico_channels";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupName"></param>
		public GroupInfo(string groupName)
			: this()
		{
			this.Name = groupName;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public int LINK { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public string Image { get; set; }

		private List<int> channels;
		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public int[] Channels
		{
			get { return channels.ToArray(); }
			set { channels = (value ?? new int[0]).ToList(); }
		}
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("#{0} {1}", this.LINK, this.Name);
		}
		#endregion

	}
}
