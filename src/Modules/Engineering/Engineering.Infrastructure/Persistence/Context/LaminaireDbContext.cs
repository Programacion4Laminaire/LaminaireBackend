using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Engineering.Infrastructure.Persistence.Context;

public class LaminaireDbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public LaminaireDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("ReqLaminaireConnection")!;
    }

    public IDbConnection CreateConnection => new SqlConnection(_connectionString);
}
