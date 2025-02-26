using Microsoft.Data.SqlClient;
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
            return new SqlConnection(_connectionString);
        }
    }
}
