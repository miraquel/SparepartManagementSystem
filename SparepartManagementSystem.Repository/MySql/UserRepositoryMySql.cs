using System.Data;
using System.Data.SqlTypes;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;
using static System.String;

namespace SparepartManagementSystem.Repository.MySql;

internal class UserRepositoryMySql : IUserRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;

    public UserRepositoryMySql(IDbConnection sqlConnection, IDbTransaction dbTransaction)
    {
        _sqlConnection = sqlConnection;
        _dbTransaction = dbTransaction;
    }

    public async Task Add(User entity)
    {
        const string sql = """
                           INSERT INTO Users (Username, FirstName, LastName, Email, IsAdministrator, IsEnabled, LastLogin, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES (@Username, @FirstName, @LastName, @Email, @IsAdministrator, @IsEnabled, @LastLogin, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
    }

    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM Users WHERE UserId = @UserId";
        await _sqlConnection.ExecuteAsync(sql, new { UserId = id }, _dbTransaction);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        const string sql = "SELECT * FROM Users";
        return await _sqlConnection.QueryAsync<User>(sql, transaction: _dbTransaction);
    }

    public async Task<User> GetById(int id, bool forUpdate = false)
    {
        const string sql = """
                           SELECT * FROM Users
                           WHERE UserId = @UserId
                           """;
        const string sqlForUpdate = """
                                    SELECT * FROM Users
                                    WHERE UserId = @UserId
                                    FOR UPDATE
                                    """;
        return await _sqlConnection.QueryFirstAsync<User>(forUpdate ? sqlForUpdate : sql, new { UserId = id }, _dbTransaction);
    }

    public Task<IEnumerable<User>> GetByParams(User entity)
    {
        var builder = new SqlBuilder();

        if (entity.UserId > 0)
        {
            builder.Where("UserId = @UserId", new { entity.UserId });
        }

        if (!IsNullOrEmpty(entity.Username))
        {
            builder.Where("Username = @Username", new { entity.Username });
        }

        if (!IsNullOrEmpty(entity.FirstName))
        {
            builder.Where("FirstName = @FirstName", new { entity.FirstName });
        }

        if (!IsNullOrEmpty(entity.LastName))
        {
            builder.Where("LastName = @LastName", new { entity.LastName });
        }

        if (!IsNullOrEmpty(entity.Email))
        {
            builder.Where("Email = @Email", new { entity.Email });
        }

        if (entity.IsAdministrator is not null)
        {
            builder.Where("IsAdministrator = @IsAdministrator", new { entity.IsAdministrator });
        }

        if (entity.IsEnabled is not null)
        {
            builder.Where("IsEnabled = @IsEnabled", new { entity.IsEnabled });
        }

        if (entity.LastLogin > SqlDateTime.MinValue.Value)
        {
            builder.Where("CAST(LastLogin AS date) = CAST(@LastLogin AS date)", new { entity.LastLogin });
        }

        if (!IsNullOrEmpty(entity.CreatedBy))
        {
            builder.Where("CreatedBy = @CreatedBy", new { entity.CreatedBy });
        }

        if (entity.CreatedDateTime > SqlDateTime.MinValue.Value)
        {
            builder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)", new { entity.CreatedDateTime });
        }

        if (!IsNullOrEmpty(entity.ModifiedBy))
        {
            builder.Where("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (entity.ModifiedDateTime > SqlDateTime.MinValue.Value)
        {
            builder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { entity.ModifiedDateTime });
        }

        const string sql = "SELECT * FROM Users /**where**/";
        var template = builder.AddTemplate(sql);
        return _sqlConnection.QueryAsync<User>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(User entity)
    {
        var builder = new SqlBuilder();

        if (!IsNullOrEmpty(entity.Username))
        {
            builder.Set("Username = @Username", new { entity.Username });
        }

        if (!IsNullOrEmpty(entity.FirstName))
        {
            builder.Set("FirstName = @FirstName", new { entity.FirstName });
        }

        if (!IsNullOrEmpty(entity.LastName))
        {
            builder.Set("LastName = @LastName", new { entity.LastName });
        }

        if (!IsNullOrEmpty(entity.Email))
        {
            builder.Set("Email = @Email", new { entity.Email });
        }

        if (entity.IsAdministrator is not null)
        {
            builder.Set("IsAdministrator = @IsAdministrator", new { entity.IsAdministrator });
        }

        if (entity.IsEnabled is not null)
        {
            builder.Set("IsEnabled = @IsEnabled", new { entity.IsEnabled });
        }

        if (entity.LastLogin > SqlDateTime.MinValue.Value)
        {
            builder.Set("LastLogin = @LastLogin", new { entity.LastLogin });
        }

        if (!IsNullOrEmpty(entity.ModifiedBy))
        {
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (entity.ModifiedDateTime > SqlDateTime.MinValue.Value)
        {
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }

        builder.Where("UserId = @UserId", new { entity.UserId });

        const string sql = "UPDATE Users /**set**/ /**where**/";
        var template = builder.AddTemplate(sql);
        
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
    }
    public Task<int> GetLastInsertedId()
    {
        return _sqlConnection.ExecuteScalarAsync<int>("SELECT LAST_INSERT_ID()", transaction: _dbTransaction);
    }
    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;

    public async Task<IEnumerable<User>> GetAllWithRoles()
    {
        const string sql = """
                           SELECT
                           u.UserId, u.Username, u.FirstName, u.LastName, u.Email, u.IsAdministrator, u.IsEnabled, u.LastLogin, u.CreatedBy, u.CreatedDateTime, u.ModifiedBy, u.ModifiedDateTime,
                           r.RoleId, r.RoleName, r.Description, r.CreatedBy, r.CreatedDateTime, r.ModifiedBy, r.ModifiedDateTime
                           FROM Users u
                           LEFT OUTER JOIN UserRoles ur ON ur.UserId = u.UserId
                           LEFT OUTER JOIN Roles r ON r.RoleId = ur.RoleId
                           """;

        var result = new List<User>();

        await _sqlConnection.QueryAsync<User, Role?, User>(sql, (user, role) =>
        {
            if (result.TrueForAll(x => x.UserId != user.UserId))
            {
                result.Add(user);
                result[^1].Roles = new List<Role>();
            }

            if (role != null) result[^1].Roles.Add(role);
            return user;
        }, splitOn: "RoleId", transaction: _dbTransaction);

        return result;
    }

    public async Task<User> GetByIdWithRoles(int id)
    {
        const string sql = """
                           SELECT
                           u.UserId, u.Username, u.FirstName, u.LastName, u.Email, u.IsAdministrator, u.IsEnabled, u.LastLogin, u.CreatedBy, u.CreatedDateTime, u.ModifiedBy, u.ModifiedDateTime,
                           r.RoleId, r.RoleName, r.Description, r.CreatedBy, r.CreatedDateTime, r.ModifiedBy, r.ModifiedDateTime
                           FROM Users u
                           LEFT OUTER JOIN UserRoles ur ON ur.UserId = u.UserId
                           LEFT OUTER JOIN Roles r ON r.RoleId = ur.RoleId
                           WHERE u.UserId = @UserId
                           """;

        var result = new User();

        _ = await _sqlConnection.QueryAsync<User, Role?, User>(sql, (user, role) =>
        {
            if (result.UserId == 0)
            {
                result = user;
            }

            if (role != null) result.Roles.Add(role);
            return user;
        }, new { UserId = id }, splitOn: "RoleId", transaction: _dbTransaction);

        return result;
    }

    public Task<User> GetByUsername(string username)
    {
        const string sql = """
                           SELECT * FROM Users
                           WHERE Username = @Username
                           """;

        return _sqlConnection.QueryFirstAsync<User>(sql, new { Username = username }, _dbTransaction);
    }

    public async Task<User> GetByUsernameWithRoles(string username)
    {
        const string sql = """
                           SELECT
                           u.UserId, u.Username, u.FirstName, u.LastName, u.Email, u.IsAdministrator, u.IsEnabled, u.LastLogin, u.CreatedBy, u.CreatedDateTime, u.ModifiedBy, u.ModifiedDateTime,
                           r.RoleId, r.RoleName, r.Description, r.CreatedBy, r.CreatedDateTime, r.ModifiedBy, r.ModifiedDateTime
                           FROM Users u
                           LEFT OUTER JOIN UserRoles ur ON ur.UserId = u.UserId
                           LEFT OUTER JOIN Roles r ON r.RoleId = ur.RoleId
                           WHERE u.Username = @Username
                           """;

        var result = new User();

        _ = await _sqlConnection.QueryAsync<User, Role?, User>(sql, (user, role) =>
        {
            if (result.UserId == 0)
            {
                result = user;
            }

            if (role != null) result.Roles.Add(role);
            return user;
        }, new { Username = username }, splitOn: "RoleId", transaction: _dbTransaction);

        return result;
    }

    public async Task<User> GetByIdWithUserWarehouse(int id)
    {
        const string sql = """
                           SELECT
                           u.UserId, u.Username, u.FirstName, u.LastName, u.Email, u.IsAdministrator, u.IsEnabled, u.LastLogin, u.CreatedBy, u.CreatedDateTime, u.ModifiedBy, u.ModifiedDateTime,
                           uw.UserWarehouseId, uw.UserId, uw.InventLocationId, uw.Name, uw.IsDefault, uw.CreatedBy, uw.CreatedDateTime, uw.ModifiedBy, uw.ModifiedDateTime
                           FROM Users u
                           LEFT OUTER JOIN UserWarehouses uw ON uw.UserId = u.UserId
                           WHERE u.UserId = @UserId
                           """;

        var result = new User();

        _ = await _sqlConnection.QueryAsync<User, UserWarehouse?, User>(sql, (user, userWarehouse) =>
        {
            if (result.UserId == 0)
            {
                result = user;
            }

            if (userWarehouse != null) result.UserWarehouses.Add(userWarehouse);
            return user;
        }, new { UserId = id }, splitOn: "UserWarehouseId", transaction: _dbTransaction);

        return result;
    }
}