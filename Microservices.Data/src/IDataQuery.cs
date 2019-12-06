using System;
using System.Data;
using System.Linq;

using NHibernate;

namespace Microservices.Data
{
	/// <summary>
	/// Запрос к БД.
	/// </summary>
	public interface IDataQuery : IDisposable
	{

		#region Properties
		/// <summary>
		/// {Get} Сессия.
		/// </summary>
		ISession Session { get; }

		/// <summary>
		/// {Get} Транзакция.
		/// </summary>
		ITransaction Transaction { get; }
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="link"></param>
		/// <returns></returns>
		T Get<T>(int link) where T : class;

		/// <summary>
		/// Открыть запрос на получение сущностей.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IQueryOver<T, T> Open<T>() where T : class;

		/// <summary>
		/// Открыть запрос на получение сущностей.
		/// </summary>
		/// <param name="hql"></param>
		/// <returns></returns>
		IQuery Open(string hql);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		ISQLQuery CreateSQLQuery(string sql);

		/// <summary>
		/// 
		/// </summary>
		void End();

		/// <summary>
		/// 
		/// </summary>
		void Cancel();
		#endregion

	}
}
