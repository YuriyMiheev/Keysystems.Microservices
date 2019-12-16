using System;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public sealed class GroupInfoUpdateParams
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		private GroupInfoUpdateParams()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="link"></param>
		public GroupInfoUpdateParams(int link)
		{
			this.GroupLink = link;
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get}
		/// </summary>
		public int GroupLink { get; private set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public string Image { get; set; }
		#endregion

	}
}
