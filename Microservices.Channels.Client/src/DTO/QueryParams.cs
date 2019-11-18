using System;

namespace Microservices.Channels.Client
{
	/// <summary>
	/// Параметры запроса сообщений.
	/// </summary>
	[Serializable]
	public class QueryParams
	{

		#region Properties
		private string query;
		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public string Query
		{
			get { return query; }
			set { query = value; }
		}

		private object _params;
		/// <summary>
		/// {Get,Set} 
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

	}
}
