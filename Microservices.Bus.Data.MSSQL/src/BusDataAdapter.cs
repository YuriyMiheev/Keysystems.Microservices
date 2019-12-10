using System.Text;

using Microservices.Data;
using Microservices.Data.MSSQL;

namespace Microservices.Bus.Data.MSSQL
{
	public class BusDataAdapter : MessageDataAdapterBase, IBusDataAdapter
	{

		#region Ctor
		/// <summary>
		/// Создание экземпляра.
		/// </summary>
		/// <param name="database"></param>
		public BusDataAdapter(IDatabase database)
			: base(database)
		{ }

		/// <summary>
		/// Создание экземпляра.
		/// </summary>
		/// <param name="dbContext"></param>
		public BusDataAdapter(DbContext dbContext)
				: base(dbContext)
		{ }
		#endregion


		protected override MessageBodyStreamBase ConstructMessageBodyStream(int msgLink, UnitOfWork work, DataStreamMode mode, Encoding encoding)
		{
			return new MessageBodyStream(this.DbContext, work, mode, msgLink, encoding);
		}

		protected override MessageContentStreamBase ConstructMessageContentStream(int contentLink, UnitOfWork work, DataStreamMode mode, Encoding encoding)
		{
			return new MessageContentStream(this.DbContext, work, mode, contentLink, encoding);
		}
	}
}
