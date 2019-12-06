using System;
using System.Linq.Expressions;

using FluentNHibernate.Mapping;

namespace Microservices.Data.Mappings
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class ClassMapBase<T> : ClassMap<T>
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		protected ClassMapBase()
		{
			DefineTable();
			DefineKey();
			DefineColumns();
		}
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		protected abstract void DefineTable();

		/// <summary>
		/// 
		/// </summary>
		protected abstract void DefineKey();

		/// <summary>
		/// 
		/// </summary>
		protected abstract void DefineColumns();
		#endregion

	}
}
