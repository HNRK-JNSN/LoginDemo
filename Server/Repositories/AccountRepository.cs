using LoginDemo.Shared.Models;
using Dapper;

namespace LoginDemo.Server.Repositories
{
    public interface IAccountRepository
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<User?> AuthenticateUser (Login model);
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly DapperContext _context;

        public AccountRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var query = @"SELECT userid AS id, name, emailaddress AS EmailAddress, role FROM ""user"" ";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<User>(query);
                return users.ToList();
            }
        }

        public async Task<User?> AuthenticateUser (Login model)
        {
            var parameters = new { username = model.EmailAddress, password = model.Password };

            var query = "SELECT authenticate_user(@username, @password)";

            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QueryFirstAsync<User>(query, parameters);
                return user;
            }
        }
    }

}