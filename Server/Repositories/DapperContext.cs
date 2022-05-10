using Npgsql;
using System.Data;

namespace LoginDemo.Server.Repositories 
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("pgconnection");

            Console.WriteLine($"{_connectionString}");
        }

        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString); 
    }
}