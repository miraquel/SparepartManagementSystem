using System.Data;
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
        entity.AcceptChanges();
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
        const string sql = "SELECT * FROM Users WHERE UserId = @UserId";
        const string sqlForUpdate = "SELECT * FROM Users WHERE UserId = @UserId FOR UPDATE";
        var result = await _sqlConnection.QueryFirstAsync<User>(forUpdate ? sqlForUpdate : sql, new { UserId = id }, _dbTransaction);
        result.AcceptChanges();
        return result;
    }

    public Task<IEnumerable<User>> GetByParams(Dictionary<string, string> parameters)
    {
        var builder = new SqlBuilder();

        if (parameters.TryGetValue("userId", out var userIdString) && int.TryParse(userIdString, out var userId)) 
        {
            builder.Where("UserId = @UserId", new { UserId = userId });
        }

        if (parameters.TryGetValue("username", out var username) && !IsNullOrEmpty(username))
        {
            builder.Where("Username LIKE @Username", new { Username = $"%{username}%" });
        }

        if (parameters.TryGetValue("firstName", out var firstName) && !IsNullOrEmpty(firstName))
        {
            builder.Where("FirstName LIKE @FirstName", new { FirstName = $"%{firstName}%" });
        }

        if (parameters.TryGetValue("lastName", out var lastName) && !IsNullOrEmpty(lastName))
        {
            builder.Where("LastName LIKE @LastName", new { LastName = $"%{lastName}%" });
        }

        if (parameters.TryGetValue("email", out var email) && !IsNullOrEmpty(email))
        {
            builder.Where("Email LIKE @Email", new { Email = $"%{email}%" });
        }

        if (parameters.TryGetValue("isAdministrator", out var isAdministratorString) && bool.TryParse(isAdministratorString, out var isAdministrator))
        {
            builder.Where("IsAdministrator = @IsAdministrator", new { IsAdministrator = isAdministrator });
        }

        if (parameters.TryGetValue("isEnabled", out var isEnabledString) && bool.TryParse(isEnabledString, out var isEnabled))
        {
            builder.Where("IsEnabled = @IsEnabled", new { IsEnabled = isEnabled });
        }

        if (parameters.TryGetValue("lastLogin", out var lastLoginString) && DateTime.TryParse(lastLoginString, out var lastLogin))
        {
            builder.Where("CAST(LastLogin AS date) = CAST(@LastLogin AS date)", new { LastLogin = lastLogin });
        }

        if (parameters.TryGetValue("createdBy", out var createdBy) && !IsNullOrEmpty(createdBy))
        {
            builder.Where("CreatedBy = @CreatedBy", new { CreatedBy = createdBy });
        }

        if (parameters.TryGetValue("createdDateTime", out var createdDateTimeString) && DateTime.TryParse(createdDateTimeString, out var createdDateTime))
        {
            builder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)", new { CreatedDateTime = createdDateTime });
        }

        if (parameters.TryGetValue("modifiedBy", out var modifiedBy) && !IsNullOrEmpty(modifiedBy))
        {
            builder.Where("ModifiedBy = @ModifiedBy", new { ModifiedBy = modifiedBy });
        }

        if (parameters.TryGetValue("modifiedDateTime", out var modifiedDateTimeString) && DateTime.TryParse(modifiedDateTimeString, out var modifiedDateTime))
        {
            builder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { ModifiedDateTime = modifiedDateTime });
        }

        const string sql = "SELECT * FROM Users /**where**/";
        var template = builder.AddTemplate(sql);
        return _sqlConnection.QueryAsync<User>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(User entity)
    {
        var builder = new SqlBuilder();

        if (!Equals(entity.OriginalValue(nameof(entity.Username)), entity.Username))
        {
            builder.Set("Username = @Username", new { entity.Username });
        }

        if (!Equals(entity.OriginalValue(nameof(entity.FirstName)), entity.FirstName))
        {
            builder.Set("FirstName = @FirstName", new { entity.FirstName });
        }

        if (!Equals(entity.OriginalValue(nameof(entity.LastName)), entity.LastName))
        {
            builder.Set("LastName = @LastName", new { entity.LastName });
        }

        if (!Equals(entity.OriginalValue(nameof(entity.Email)), entity.Email))
        {
            builder.Set("Email = @Email", new { entity.Email });
        }

        if (!Equals(entity.OriginalValue(nameof(entity.IsAdministrator)), entity.IsAdministrator))
        {
            builder.Set("IsAdministrator = @IsAdministrator", new { entity.IsAdministrator });
        }

        if (!Equals(entity.OriginalValue(nameof(entity.IsEnabled)), entity.IsEnabled))
        {
            builder.Set("IsEnabled = @IsEnabled", new { entity.IsEnabled });
        }

        if (!Equals(entity.OriginalValue(nameof(entity.LastLogin)), entity.LastLogin))
        {
            builder.Set("LastLogin = @LastLogin", new { entity.LastLogin });
        }

        if (!Equals(entity.OriginalValue(nameof(entity.ModifiedBy)), entity.ModifiedBy))
        {
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (!Equals(entity.OriginalValue(nameof(entity.ModifiedDateTime)), entity.ModifiedDateTime))
        {
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }

        builder.Where("UserId = @UserId", new { entity.UserId });

        const string sql = "UPDATE Users /**set**/ /**where**/";
        var template = builder.AddTemplate(sql);
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
        entity.AcceptChanges();
    }
    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;

    public async Task<IEnumerable<User>> GetAllWithRoles()
    {
        const string sql = """
                           SELECT
                           u.*,
                           r.*
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

            if (role != null)
            {
                result[^1].Roles.Add(role);
            }

            return user;
        }, splitOn: "RoleId", transaction: _dbTransaction);

        return result;
    }

    public async Task<User> GetByIdWithRoles(int id)
    {
        const string sql = """
                           SELECT
                           u.*,
                           r.*
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

            if (role != null)
            {
                result.Roles.Add(role);
            }

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
                           u.*,
                           r.*
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

            if (role != null)
            {
                result.Roles.Add(role);
            }

            return user;
        }, new { Username = username }, splitOn: "RoleId", transaction: _dbTransaction);

        return result;
    }

    public async Task<User> GetByIdWithUserWarehouse(int id)
    {
        const string sql = """
                           SELECT
                           u.*,
                           uw.*
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

            if (userWarehouse != null)
            {
                result.UserWarehouses.Add(userWarehouse);
            }

            return user;
        }, new { UserId = id }, splitOn: "UserWarehouseId", transaction: _dbTransaction);

        return result;
    }
}