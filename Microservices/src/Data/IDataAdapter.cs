using System.Data;

namespace Microservices.Data
{
	/// <summary>
	/// Адаптер доступа к БД.
	/// </summary>
	public interface IDataAdapter
	{

		#region Properties
		DbContext DbContext { get; }

		int ExecuteTimeout { get; set; }
		#endregion


		#region Methods
		bool CheckConnection(out ConnectionException error);

		IDataQuery OpenQuery();

		IDataQuery OpenQuery(IsolationLevel transaction);

		UnitOfWork BeginWork();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		int ExecuteUpdate(string sql);
		#endregion

	}
}
