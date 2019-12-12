using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservices.Data
{
	/// <summary>
	/// Параметры выборки сообщений.
	/// </summary>
	/// <remarks>http://ayende.com/blog/4023/nhibernate-queries-examples</remarks>
	[Serializable]
	public class QueryParams
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public QueryParams()
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		public QueryParams(string query)
		{
			#region Validate parameters
			if ( String.IsNullOrEmpty(query) )
				throw new ArgumentException("Пустой текст запроса.", "query");
			#endregion

			this.Query = query;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		public QueryParams(string query, object parameters)
			: this(query)
		{
			this.Params = parameters;
		}
		#endregion


		#region Properties
		private string query;
		/// <summary>
		/// {Get,Set} Текст запроса:
		/// 1.Именованные параметры - "FROM Message m WHERE m.Name=:name";
		/// 2.Позиционные параметры - "FROM Message m WHERE m.Name=?";
		/// </summary>
		public string Query
		{
			get { return query; }
			set { query = value; }
		}

		private object _params;
		/// <summary>
		/// {Get,Set} Параметры запроса:
		/// 1.Объект-шаблон именованных параметров (new { name = "xxx"});
		/// 2.Массив объектов позиционных параметров (object[] { "xxx" });
		/// 3.Словарь именованных параметров (IDictionary[string, object]);
		/// </summary>
		public object Params
		{
			get { return _params; }
			set { _params = value; }
		}

		private int? skip;
		/// <summary>
		/// {Get,Set} Пропустить N записей.
		/// </summary>
		public int? Skip
		{
			get { return skip; }
			set { skip = value; }
		}

		private int? take;
		/// <summary>
		/// {Get,Set} Взять N записей.
		/// </summary>
		public int? Take
		{
			get { return take; }
			set { take = value; }
		}
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Query;
		}
		#endregion

	}
}
