using Dapper;
using MinhaMinimalApiDapper.Entity;
using MinhaMinimalApiDapper.Request;
using System.Data;

namespace MinhaMinimalApiDapper.Repository
{
    public class UserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Guid> CreateAsync(CreateUserRequest request)
        {
            var id = Guid.NewGuid();

            const string sql = """
                INSERT INTO users (id, name, email)
                VALUES (@Id, @Name, @Email)
            """;

            await _connection.ExecuteAsync(sql, new
            {
                Id = id,
                request.Name,
                request.Email
            });

            return id;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            const string sql = """
                SELECT id, name, email
                FROM users
                WHERE id = @Id
            """;

            return await _connection
                .QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }
    }
}
