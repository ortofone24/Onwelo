using System.Data;
using Microsoft.Data.SqlClient;

namespace Onwelo.Api.Data
{
    public class DapperContext
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public DapperContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = configuration.GetConnectionString("SqlConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(connectionString);
    }
}
