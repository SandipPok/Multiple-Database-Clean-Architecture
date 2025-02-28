using MySql.Data.MySqlClient;
using System.Data;

namespace CleanArchitecture.Infrastructure.Data
{
    public class MySqlDbContext
    {
        private readonly string _connectionString;
        public MySqlDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IDbConnection CreateConnection()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
