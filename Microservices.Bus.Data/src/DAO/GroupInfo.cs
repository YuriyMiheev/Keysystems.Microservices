namespace Microservices.Bus.Data.DAO
{
	/// <summary>
	/// 
	/// </summary>
	public class GroupInfo
	{

		#region Properties
		/// <summary>
		/// {Get} Внутренний ID.
		/// </summary>
		public virtual int LINK { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public virtual string Image { get; set; }
		#endregion

	}
}
