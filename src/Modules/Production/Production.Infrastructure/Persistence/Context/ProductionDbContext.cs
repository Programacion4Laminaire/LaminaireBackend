using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Production.Infrastructure.Persistence.Context
{
    public class ProductionDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ProductionDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("PrductionLaminaireConnection")!;
        }

        public IDbConnection CreateConnection => new SqlConnection(_connectionString);
    }
}
