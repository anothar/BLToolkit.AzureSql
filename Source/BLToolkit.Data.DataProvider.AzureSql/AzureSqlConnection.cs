namespace BLToolkit.Data.DataProvider.AzureSql
{
	using System.Data;
	using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

	internal class AzureSqlConnection : IDbConnection
	{
		private readonly ReliableSqlConnection reliableConnection;

		public AzureSqlConnection()
		{
			this.reliableConnection = new ReliableSqlConnection(null);
		}

		public void Dispose()
		{
			this.reliableConnection.Dispose();
		}

		public IDbTransaction BeginTransaction()
		{
			return this.reliableConnection.BeginTransaction();
		}

		public IDbTransaction BeginTransaction(IsolationLevel iso)
		{
			return this.reliableConnection.BeginTransaction(iso);
		}

		public void Close()
		{
			this.reliableConnection.Close();
		}

		public void ChangeDatabase(string databaseName)
		{
			this.reliableConnection.ChangeDatabase(databaseName);
		}

		public IDbCommand CreateCommand()
		{
			return new AzureSqlCommand(this.reliableConnection);
		}

		public void Open()
		{
			this.reliableConnection.Open();
		}

		public string ConnectionString
		{
			get { return this.reliableConnection.ConnectionString; }
			set { this.reliableConnection.ConnectionString = value; }
		}

		public int ConnectionTimeout
		{
			get { return this.reliableConnection.ConnectionTimeout; }
		}

		public string Database
		{
			get { return this.reliableConnection.Database; }
		}

		public ConnectionState State
		{
			get { return this.reliableConnection.State; }
		}
	}
}
