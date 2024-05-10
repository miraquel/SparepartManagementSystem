using System.Data;
using System.Data.SqlTypes;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.MySql;

internal class RefreshTokenRepositoryMySql : IRefreshTokenRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;

    public RefreshTokenRepositoryMySql(IDbTransaction dbTransaction, IDbConnection sqlConnection)
    {
        _dbTransaction = dbTransaction;
        _sqlConnection = sqlConnection;
    }

    public Task<RefreshToken> GetByUserIdAndToken(int userId, string token, bool forUpdate = false)
    {
        const string sql = """
                           SELECT * FROM RefreshTokens
                           WHERE UserId = @UserId AND Token = @Token
                           """;
        const string sqlForUpdate = """
                                   SELECT * FROM RefreshTokens
                                   WHERE UserId = @UserId AND Token = @Token FOR UPDATE
                                   """;
        return _sqlConnection.QueryFirstAsync<RefreshToken>(forUpdate ? sqlForUpdate : sql, new { UserId = userId, Token = token }, _dbTransaction);
    }

    public Task<IEnumerable<RefreshToken>> GetByUserId(int userId)
    {
        const string sql = """
                           SELECT * FROM RefreshTokens
                           WHERE UserId = @UserId
                           """;
        return _sqlConnection.QueryAsync<RefreshToken>(sql, new { UserId = userId }, _dbTransaction);
    }

    public async Task Revoke(int id)
    {
        const string sql = """
                           UPDATE RefreshTokens
                           SET Revoked = @Revoked
                           WHERE RefreshTokenId = @RefreshTokenId AND Revoked = @RevokedMinValue
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, new { Revoked = DateTime.Now, RefreshTokenId = id, RevokedMinValue = SqlDateTime.MinValue.Value }, _dbTransaction);
    }

    public async Task RevokeAll(int userId)
    {
        const string sql = """
                           UPDATE RefreshTokens
                           SET Revoked = @Revoked
                           WHERE UserId = @UserId AND Revoked = @RevokedMinValue
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, new { Revoked = DateTime.Now, UserId = userId, RevokedMinValue = SqlDateTime.MinValue.Value }, _dbTransaction);
    }

    public async Task Add(RefreshToken entity)
    {
        var currentDateTime = DateTime.Now;
        entity.Created = currentDateTime;

        const string sql = """
                           INSERT INTO RefreshTokens
                           (UserId, Token, Created, Expires, Revoked, ReplacedByToken)
                           VALUES (@UserId, @Token, @Created, @Expires, @Revoked, @ReplacedByToken)
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
    }

    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM RefreshTokens WHERE RefreshTokenId = @RefreshTokenId";
        await _sqlConnection.ExecuteAsync(sql, new { RefreshTokenId = id }, _dbTransaction);
    }

    public Task<IEnumerable<RefreshToken>> GetAll()
    {
        const string sql = "SELECT * FROM RefreshTokens";

        return _sqlConnection.QueryAsync<RefreshToken>(sql, null, _dbTransaction);
    }

    public Task<RefreshToken> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM RefreshTokens WHERE RefreshTokenId = @RefreshTokenId";
        const string sqlForUpdate = "SELECT * FROM RefreshTokens WHERE RefreshTokenId = @RefreshTokenId FOR UPDATE";
        return _sqlConnection.QueryFirstAsync<RefreshToken>(forUpdate ? sqlForUpdate : sql, new { RefreshTokenId = id }, _dbTransaction);
    }

    public Task<IEnumerable<RefreshToken>> GetByParams(RefreshToken entity)
    {
        var builder = new SqlBuilder();

        if (entity.RefreshTokenId > 0)
        {
            builder.Where("RefreshTokenId = @RefreshTokenId", new { entity.RefreshTokenId });
        }

        if (entity.UserId > 0)
        {
            builder.Where("UserId = @UserId", new { entity.UserId });
        }

        if (!string.IsNullOrEmpty(entity.Token))
        {
            builder.Where("Token LIKE @Token", new { Token = $"%{entity.Token}%" });
        }

        if (entity.Expires != DateTime.MinValue)
        {
            builder.Where("Expires = @Expires", new { entity.Expires });
        }

        if (entity.Revoked != DateTime.MinValue)
        {
            builder.Where("Revoked = @Revoked", new { entity.Revoked });
        }

        if (!string.IsNullOrEmpty(entity.ReplacedByToken))
        {
            builder.Where("ReplacedByToken LIKE @ReplacedByToken", new { ReplacedByToken = $"%{entity.ReplacedByToken}%" });
        }

        builder.OrderBy("UserId");

        const string sql = "SELECT * FROM RefreshTokens /**where**/ /**orderby**/";
        var template = builder.AddTemplate(sql);

        return _sqlConnection.QueryAsync<RefreshToken>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(RefreshToken entity)
    {
        const string sql = """
                           UPDATE RefreshTokens
                           SET UserId = @UserId,
                           Token = @Token,
                           Expires = @Expires,
                           Revoked = @Revoked,
                           ReplacedByToken = @ReplacedByToken
                           WHERE RefreshTokenId = @RefreshTokenId
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
    }
    public Task<int> GetLastInsertedId()
    {
        return _sqlConnection.ExecuteScalarAsync<int>("SELECT LAST_INSERT_ID()", transaction: _dbTransaction);
    }
    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
}