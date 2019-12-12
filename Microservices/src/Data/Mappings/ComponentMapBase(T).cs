using System;
using System.Linq.Expressions;

using FluentNHibernate.Mapping;

namespace Microservices.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class ComponentMapBase<T> : ComponentMap<T>
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		protected ComponentMapBase()
		{
			DefineColumns();
		}
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		protected abstract void DefineColumns();
		#endregion

	}
}
