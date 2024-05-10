using System.Data;
using System.Data.SqlTypes;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;
using static System.String;

namespace SparepartManagementSystem.Repository.MySql;

internal class RoleRepositoryMySql : IRoleRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;

    public RoleRepositoryMySql(IDbConnection sqlConnection, IDbTransaction dbTransaction)
    {
        _sqlConnection = sqlConnection;
        _dbTransaction = dbTransaction;
    }

    public async Task Add(Role entity)
    {
        const string sql = """
                           INSERT INTO Roles
                           (RoleName, Description, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES (@RoleName, @Description, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
    }

    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM Roles WHERE RoleId = @RoleId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { RoleId = id }, _dbTransaction);
    }

    public async Task<IEnumerable<Role>> GetAll()
    {
        const string sql = """
                           SELECT
                           RoleId, RoleName, Description, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime
                           FROM Roles
                           """;
        return await _sqlConnection.QueryAsync<Role>(sql, transaction: _dbTransaction);
    }

    public async Task<Role> GetById(int id, bool forUpdate = false)
    {
        const string sql = """
                           SELECT RoleId, RoleName, Description, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime
                           FROM Roles
                           WHERE RoleId = @RoleId
                           """;
        const string sqlForUpdate = """
                                    SELECT RoleId, RoleName, Description, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime
                                    FROM Roles
                                    WHERE RoleId = @RoleId
                                    FOR UPDATE
                                    """;
        return await _sqlConnection.QueryFirstAsync<Role>(forUpdate ? sqlForUpdate : sql, new { RoleId = id }, _dbTransaction);
    }

    public async Task<IEnumerable<Role>> GetByParams(Role entity)
    {
        var builder = new SqlBuilder();

        if (entity.RoleId > 0)
        {
            builder.Where("RoleId = @RoleId", new { entity.RoleId });
        }

        if (!IsNullOrEmpty(entity.RoleName))
        {
            builder.Where("RoleName LIKE @RoleName", new { RoleName = $"%{entity.RoleName}%" });
        }

        if (!IsNullOrEmpty(entity.Description))
        {
            builder.Where("Description LIKE @Description", new { Description = $"%{entity.Description}%" });
        }

        if (!IsNullOrEmpty(entity.CreatedBy))
        {
            builder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{entity.CreatedBy}%" });
        }

        if (entity.CreatedDateTime > SqlDateTime.MinValue.Value)
        {
            builder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)", new { entity.CreatedDateTime });
        }

        if (!IsNullOrEmpty(entity.ModifiedBy))
        {
            builder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{entity.ModifiedBy}%" });
        }

        if (entity.ModifiedDateTime > SqlDateTime.MinValue.Value)
        {
            builder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { entity.ModifiedDateTime });
        }

        const string sql = "SELECT * FROM Roles /**where**/";
        var template = builder.AddTemplate(sql);
        return await _sqlConnection.QueryAsync<Role>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(Role entity)
    {
        var builder = new SqlBuilder();

        if (!IsNullOrEmpty(entity.RoleName))
        {
            builder.Set("RoleName = @RoleName", new { entity.RoleName });
        }

        if (!IsNullOrEmpty(entity.Description))
        {
            builder.Set("Description = @Description", new { entity.Description });
        }

        if (!IsNullOrEmpty(entity.ModifiedBy))
        {
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (entity.ModifiedDateTime > DateTime.MinValue)
        {
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }

        builder.Where("RoleId = @RoleId", new { entity.RoleId });

        const string sql = "UPDATE Roles /**set**/ /**where**/";

        var template = builder.AddTemplate(sql);
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
    }
    public Task<int> GetLastInsertedId()
    {
        return _sqlConnection.ExecuteScalarAsync<int>("SELECT LAST_INSERT_ID()", transaction: _dbTransaction);
    }

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;

    public async Task AddUser(int roleId, int userId)
    {
        const string sql = """
                           INSERT INTO UserRoles
                           (UserId, RoleId)
                           VALUES (@UserId, @RoleId)
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId }, _dbTransaction);
    }

    public async Task DeleteUser(int roleId, int userId)
    {
        const string sql = """
                           DELETE FROM UserRoles
                           WHERE UserId = @UserId AND RoleId = @RoleId
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId }, _dbTransaction);
    }

    public async Task<IEnumerable<Role>> GetAllWithUsers()
    {
        const string sql = """
                           SELECT
                           r.RoleId, r.RoleName, r.Description, r.CreatedBy, r.CreatedDateTime, r.ModifiedBy, r.ModifiedDateTime,
                           u.UserId, u.Username, u.FirstName, u.LastName, u.Email, u.CreatedBy, u.CreatedDateTime, u.ModifiedBy, u.ModifiedDateTime
                           FROM Roles r
                           LEFT OUTER JOIN UserRoles ur ON ur.RoleId = r.RoleId
                           LEFT OUTER JOIN Users u ON u.UserId = ur.UserId
                           """;

        var result = new List<Role>();

        _ = await _sqlConnection.QueryAsync<Role, User?, Role>(sql, (role, user) =>
        {
            if (result.TrueForAll(x => x.RoleId != role.RoleId))
            {
                result.Add(role);
                result[^1].Users = new List<User>();
            }

            if (user != null) result[^1].Users.Add(user);
            return role;
        }, transaction: _dbTransaction, splitOn: "UserId");

        return result;
    }

    public async Task<Role> GetByIdWithUsers(int id)
    {
        const string sql = """
                           SELECT r.RoleId, r.RoleName, r.Description, r.CreatedBy, r.CreatedDateTime, r.ModifiedBy, r.ModifiedDateTime,
                           u.UserId, u.Username, u.FirstName, u.LastName, u.Email, u.CreatedBy, u.CreatedDateTime, u.ModifiedBy, u.ModifiedDateTime
                           FROM Roles r
                           LEFT OUTER JOIN UserRoles ur ON ur.RoleId = r.RoleId
                           LEFT OUTER JOIN Users u ON u.UserId = ur.UserId
                           WHERE r.RoleId = @RoleId
                           """;

        var result = new Role();

        _ = await _sqlConnection.QueryAsync<Role, User?, Role>(sql, (role, user) =>
        {
            if (result.RoleId == 0)
            {
                result = role;
            }

            if (user != null) result.Users.Add(user);
            return role;
        }, new { RoleId = id }, _dbTransaction, splitOn: "UserId");

        return result;
    }
}