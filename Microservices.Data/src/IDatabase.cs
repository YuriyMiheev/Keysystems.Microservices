using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Microservices.Data
{
	public interface IDatabase
	{

		string Provider { get; }

		string Schema { get; set; }

		string ConnectionString { get; set; }

		int ConnectionTimeout { get; set; }


		bool TryConnect(out ConnectionException error);

		DbConnection OpenNewConnection();

		DbContext Open();

		void Close();

		DbContext ValidateSchema();

		DbContext CreateOrUpdateSchema();

		DbContext RecreateSchema();

	}
}