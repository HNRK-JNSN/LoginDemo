using LoginDemo.Shared.Models;
using Dapper;

namespace LoginDemo.Server.Repositories
{
    public interface IAccountRepository
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<User?> AuthenticateUser (Login model);

        public Task<int> AddUser(AddUser model);
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

        public async Task<int> AddUser(AddUser model)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@name", model.Name);
            parameters.Add("@email", model.EmailAddress);
            parameters.Add("@role", model.Role);
            parameters.Add("@password", model.Password);

            var query = "call public.create_user(@name, @email, @role, @password)";
            int affected = 0;

            using (var connection = _context.CreateConnection())
            {
                try {
                    var res = await connection.ExecuteAsync(query, parameters);
                    affected=1;
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return affected;
        }

        public async Task<User?> AuthenticateUser (Login model)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@email", model.EmailAddress);
            parameters.Add("@password", model.Password);

            var query = "SELECT * FROM authenticate_user(@email, @password)";
            User? user = null;

            using (var connection = _context.CreateConnection())
            {
                try 
                {
                    user = await connection.QuerySingleOrDefaultAsync<User>(query, parameters);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return user;
        }
    }

}