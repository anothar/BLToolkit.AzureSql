using System;
using System.Data.SqlClient;

namespace Tests
{
    /// <summary>
    /// SqlServer database management
    /// </summary>
    public class ServerDbManager
    {
        private String _connectionString;


        public ServerDbManager(String connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Create database
        /// </summary>
        /// <param name="name"></param>
        public void CreateDatabase(String name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CREATE DATABASE " + name;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        /// <summary>
        /// Delete database
        /// </summary>
        /// <param name="name"></param>
        public void DeleteDatabase(String name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select @@SPID";
                    int spid = 0;
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        spid = reader.GetInt16(0);
                    }
                    //kill all connections to database
                    command.CommandText =
                        String.Format("DECLARE @kill varchar(8000) = '';SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), spid) + ';'" +
                        "FROM master..sysprocesses WHERE dbid = db_id('{0}') and spid > 50 and spid!={1}; EXEC(@kill);", name, spid);
                    command.ExecuteNonQuery();
                    command.CommandText = String.Format("alter database {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;DROP DATABASE {0}", name);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
