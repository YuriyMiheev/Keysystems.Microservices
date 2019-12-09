namespace Microservices.Bus.Data.DAO
{
	/// <summary>
	/// Связь группа/канал.
	/// </summary>
	public class GroupChannelMap
	{

		#region Properties
		/// <summary>
		/// {Get,Set} Внутренний ID.
		/// </summary>
		public virtual int LINK { get; set; }

		/// <summary>
		/// {Get,Set} Группа.
		/// </summary>
		public virtual int? GroupLINK { get; set; }

		/// <summary>
		/// {Get} Канал.
		/// </summary>
		public virtual int? ChannelLINK { get; set; }
		#endregion

	}
}
