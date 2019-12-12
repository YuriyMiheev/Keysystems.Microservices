using System;

namespace Microservices.Bus.Data.DAO
{
	/// <summary>
	/// 
	/// </summary>
	public class ChannelProperty
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
		public virtual string DefaultValue { get; set; }

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
		public virtual bool? ReadOnly { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual bool? Secret { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual ChannelInfo Channel { get; set; }
		#endregion

	}
}
