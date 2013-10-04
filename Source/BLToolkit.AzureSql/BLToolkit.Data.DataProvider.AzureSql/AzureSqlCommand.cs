namespace BLToolkit.Data.DataProvider.AzureSql
{
	using System.Data;
	using System.Data.SqlClient;
	using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

	internal class AzureSqlCommand : IDbCommand
	{
		private readonly SqlCommand command;

		public AzureSqlCommand(ReliableSqlConnection reliableConnection)
		{
			this.command = reliableConnection.CreateCommand();
			this.Connection = reliableConnection;
		}

		public void Dispose()
		{
			this.command.Dispose();
		}

		public void Prepare()
		{
			this.command.Prepare();
		}

		public void Cancel()
		{
			this.command.Prepare();
		}

		public IDbDataParameter CreateParameter()
		{
			return this.command.CreateParameter();
		}

		public int ExecuteNonQuery()
		{
			return this.command.ExecuteNonQueryWithRetry();
		}

		public IDataReader ExecuteReader()
		{
			return this.command.ExecuteReaderWithRetry();
		}

		public IDataReader ExecuteReader(CommandBehavior behavior)
		{
			return this.command.ExecuteReaderWithRetry(behavior);
		}

		public object ExecuteScalar()
		{
			return this.command.ExecuteScalarWithRetry();
		}

		public IDbConnection Connection { get; set; }

		public IDbTransaction Transaction
		{
			get { return this.command.Transaction; }
			set { this.command.Transaction = (SqlTransaction) value; }
		}

		public string CommandText
		{
			get { return this.command.CommandText; }
			set { this.command.CommandText = value; }
		}

		public int CommandTimeout
		{
			get { return this.command.CommandTimeout; }
			set { this.command.CommandTimeout = value; }
		}

		public CommandType CommandType
		{
			get { return this.command.CommandType; }
			set { this.command.CommandType = value; }
		}

		public IDataParameterCollection Parameters
		{
			get { return this.command.Parameters; }
		}

		public UpdateRowSource UpdatedRowSource
		{
			get { return this.command.UpdatedRowSource; }
			set { this.command.UpdatedRowSource = value; }
		}
	}
}
