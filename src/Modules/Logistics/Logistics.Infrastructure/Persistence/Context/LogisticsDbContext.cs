using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Logistics.Infrastructure.Persistence.Context
{
    public class LogisticsDbContext
    {
        private readonly  IConfiguration _configuration;
        private readonly string _connectionString;

        public LogisticsDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("LogisticsLaminaireConnection")!;
        }

        public IDbConnection CreateConnection => new SqlConnection(_connectionString);
    }
}
