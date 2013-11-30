namespace BLToolkit.Data.DataProvider.AzureSql
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Linq;
	using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
	using Sql.SqlProvider;

	public sealed class AzureSqlDataProvider : SqlDataProviderBase
	{
		private static readonly List<Func<Type, string>> UdtTypeNameResolvers = new List<Func<Type, string>>();
		private readonly RetryPolicy commandRetryPolicy;
		private readonly RetryPolicy connectionRetryPolicy;

		static AzureSqlDataProvider()
		{
			AddUdtTypeNameResolver(ResolveStandartUdt);
		}

		public AzureSqlDataProvider()
			: this(new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(RetryStrategy.DefaultExponential))
		{
		}

		public AzureSqlDataProvider(RetryPolicy defaultRetryPolicy)
			: this(defaultRetryPolicy, defaultRetryPolicy)
		{
		}

		public AzureSqlDataProvider(RetryPolicy connectionRetryPolicy, RetryPolicy commandRetryPolicy)
		{
			this.connectionRetryPolicy = connectionRetryPolicy;
			this.commandRetryPolicy = commandRetryPolicy;
		}

		public override string Name
		{
			get { return AzureSql.ProviderName.AzureSql; }
		}

		public override bool DeriveParameters(IDbCommand command)
		{
			return base.DeriveParameters((SqlCommand) ((AzureSqlCommand) command));
		}

		public override IDbConnection CreateConnectionObject()
		{
			return new AzureSqlConnection(this.connectionRetryPolicy, this.commandRetryPolicy);
		}

		public static void AddUdtTypeNameResolver(Func<Type, string> resolver)
		{
			UdtTypeNameResolvers.Add(resolver);
		}

		private static string ResolveStandartUdt(Type type)
		{
			return type.Namespace == "Microsoft.SqlServer.Types" ? type.Name.Replace("Sql", "") : null;
		}

		public override ISqlProvider CreateSqlProvider()
		{
			return new MsSql2012SqlProvider();
		}

		public override void SetParameterValue(IDbDataParameter parameter, object value)
		{
			base.SetParameterValue(parameter, value);
			SetUdtTypeName(parameter, value);
		}

		private static void SetUdtTypeName(IDbDataParameter parameter, object value)
		{
			var sqlParameter = parameter as System.Data.SqlClient.SqlParameter;
			var valueType = value.GetType();

			if (sqlParameter != null)
			{
				sqlParameter.UdtTypeName =
					UdtTypeNameResolvers.Select(_ => _(valueType)).FirstOrDefault(_ => !string.IsNullOrEmpty(_));
			}
		}
	}
}
