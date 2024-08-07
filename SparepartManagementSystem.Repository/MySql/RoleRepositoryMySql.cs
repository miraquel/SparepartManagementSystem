﻿using System.Data;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Shared.DerivedClass;
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

    public async Task Add(Role entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null)
    {
        onBeforeAdd?.Invoke(this, new AddEventArgs(entity));
        
        const string sql = """
                           INSERT INTO Roles
                           (RoleName, Description, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES (@RoleName, @Description, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
        entity.AcceptChanges();
        
        onAfterAdd?.Invoke(this, new AddEventArgs(entity));
    }

    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM Roles WHERE RoleId = @RoleId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { RoleId = id }, _dbTransaction);
    }

    public async Task<IEnumerable<Role>> GetAll()
    {
        const string sql = "SELECT * FROM Roles";
        return await _sqlConnection.QueryAsync<Role>(sql, transaction: _dbTransaction);
    }

    public async Task<Role> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM Roles WHERE RoleId = @RoleId";
        const string sqlForUpdate = "SELECT * FROM Roles WHERE RoleId = @RoleId FOR UPDATE";
        var result =
            await _sqlConnection.QueryFirstOrDefaultAsync<Role>(forUpdate ? sqlForUpdate : sql, new { RoleId = id },
                _dbTransaction) ?? throw new InvalidOperationException($"Role with Id {id} not found");
        result.AcceptChanges();
        return result;
    }

    public async Task<IEnumerable<Role>> GetByParams(Dictionary<string, string> parameters)
    {
        var builder = new SqlBuilder();

        if (parameters.TryGetValue("roleId", out var roleIdString) && int.TryParse(roleIdString, out var roleId))
        {
            builder.Where("RoleId = @RoleId", new { RoleId = roleId });
        }

        if (parameters.TryGetValue("roleName", out var roleName) && !IsNullOrEmpty(roleName))
        {
            builder.Where("RoleName LIKE @RoleName", new { RoleName = $"%{roleName}%" });
        }

        if (parameters.TryGetValue("description", out var description) && !IsNullOrEmpty(description))
        {
            builder.Where("Description LIKE @Description", new { Description = $"%{description}%" });
        }

        if (parameters.TryGetValue("createdBy", out var createdBy) && !IsNullOrEmpty(createdBy))
        {
            builder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{createdBy}%" });
        }

        if (parameters.TryGetValue("createdDateTime", out var createdDateTimeString) &&
            DateTime.TryParse(createdDateTimeString, out var createdDateTime))
        {
            builder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)",
                new { CreatedDateTime = createdDateTime });
        }

        if (parameters.TryGetValue("modifiedBy", out var modifiedBy) && !IsNullOrEmpty(modifiedBy))
        {
            builder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{modifiedBy}%" });
        }

        if (parameters.TryGetValue("modifiedDateTime", out var modifiedDateTimeString) &&
            DateTime.TryParse(modifiedDateTimeString, out var modifiedDateTime))
        {
            builder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { modifiedDateTime });
        }

        const string sql = "SELECT * FROM Roles /**where**/";
        var template = builder.AddTemplate(sql);
        return await _sqlConnection.QueryAsync<Role>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(Role entity, EventHandler<BeforeUpdateEventArgs>? onBeforeUpdate = null,
        EventHandler<AfterUpdateEventArgs>? onAfterUpdate = null)
    {
        var builder = new CustomSqlBuilder();
        
        if (!entity.ValidateUpdate())
        {
            return;
        }

        if (entity.OriginalValue(nameof(Role.RoleName)) is not null && !Equals(entity.OriginalValue(nameof(Role.RoleName)), entity.RoleName))
        {
            builder.Set("RoleName = @RoleName", new { entity.RoleName });
        }

        if (entity.OriginalValue(nameof(Role.Description)) is not null && !Equals(entity.OriginalValue(nameof(Role.Description)), entity.Description))
        {
            builder.Set("Description = @Description", new { entity.Description });
        }

        builder.Where("RoleId = @RoleId", new { entity.RoleId });
        
        if (!builder.HasSet)
        {
            return;
        }

        onBeforeUpdate?.Invoke(this, new BeforeUpdateEventArgs(entity, builder));
        
        if (entity.OriginalValue(nameof(Role.ModifiedBy)) is not null && !Equals(entity.OriginalValue(nameof(Role.ModifiedBy)), entity.ModifiedBy))
        {
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (entity.OriginalValue(nameof(Role.ModifiedDateTime)) is not null && !Equals(entity.OriginalValue(nameof(Role.ModifiedDateTime)), entity.ModifiedDateTime))
        {
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }

        const string sql = "UPDATE Roles /**set**/ /**where**/";

        var template = builder.AddTemplate(sql);
        var rows = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
        if (rows == 0)
        {
            throw new InvalidOperationException($"Role with Id {entity.RoleId} not found");
        }
        entity.AcceptChanges();
        
        onAfterUpdate?.Invoke(this, new AfterUpdateEventArgs(entity));
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
                           r.*,
                           u.*
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

            if (user != null)
            {
                result[^1].Users.Add(user);
            }

            return role;
        }, transaction: _dbTransaction, splitOn: "UserId");

        return result;
    }

    public async Task<Role> GetByIdWithUsers(int id)
    {
        const string sql = """
                           SELECT 
                           r.*,
                           u.*
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

            if (user != null)
            {
                result.Users.Add(user);
            }

            return role;
        }, new { RoleId = id }, _dbTransaction, splitOn: "UserId");

        return result;
    }
}