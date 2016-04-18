using BLToolkit.Data;
using BLToolkit.Data.DataProvider.AzureSql;
using NUnit.Framework;
using System;

namespace Tests
{
    [TestFixture]
    public class TestBase
    {
        protected String DatabaseName = "sqlazuretests";
        private ServerDbManager _manager;

        [SetUp]
        public void SetUp()
        {
            _manager = new ServerDbManager("Data Source=.;Initial Catalog=master;" +
                "Integrated Security=true;");
            try
            {
                _manager.CreateDatabase(DatabaseName);
            }
            catch (System.Data.SqlClient.SqlException)
            {
                _manager.DeleteDatabase(DatabaseName);
                _manager.CreateDatabase(DatabaseName);
            }
        }

        public DbManager CreateContext()
        {
            return new DbManager(new AzureSqlDataProvider(), $"Data Source=.;Initial Catalog={DatabaseName};Integrated Security=true;");
        }

        [TearDown]
        public void TearDown()
        {
            _manager.DeleteDatabase(DatabaseName);
        }
    }
}
