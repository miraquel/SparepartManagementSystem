using System.Data;
using System.Data.SqlTypes;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;
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

    public async Task<RefreshToken> GetByUserIdAndToken(int userId, string token, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM RefreshTokens WHERE UserId = @UserId AND Token = @Token";
        const string sqlForUpdate = "SELECT * FROM RefreshTokens WHERE UserId = @UserId AND Token = @Token FOR UPDATE";
        return await _sqlConnection.QueryFirstOrDefaultAsync<RefreshToken>(forUpdate ? sqlForUpdate : sql,
            new { UserId = userId, Token = token }, _dbTransaction) ?? throw new Exception("Refresh token not found");
    }

    public async Task<IEnumerable<RefreshToken>> GetByUserId(int userId)
    {
        const string sql = "SELECT * FROM RefreshTokens WHERE UserId = @UserId";
        return await _sqlConnection.QueryAsync<RefreshToken>(sql, new { UserId = userId }, _dbTransaction);
    }

    public async Task Revoke(int id)
    {
        const string sql = """
                           UPDATE RefreshTokens
                           SET Revoked = @Revoked
                           WHERE RefreshTokenId = @RefreshTokenId AND Revoked = @RevokedMinValue
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql,
            new { Revoked = DateTime.Now, RefreshTokenId = id, RevokedMinValue = SqlDateTime.MinValue.Value },
            _dbTransaction);
    }

    public async Task RevokeAll(int userId)
    {
        const string sql = """
                           UPDATE RefreshTokens
                           SET Revoked = @Revoked
                           WHERE UserId = @UserId AND Revoked = @RevokedMinValue
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql,
            new { Revoked = DateTime.Now, UserId = userId, RevokedMinValue = SqlDateTime.MinValue.Value },
            _dbTransaction);
    }

    public async Task Add(RefreshToken entity)
    {
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

    public async Task<IEnumerable<RefreshToken>> GetAll()
    {
        const string sql = "SELECT * FROM RefreshTokens";
        return await _sqlConnection.QueryAsync<RefreshToken>(sql, null, _dbTransaction);
    }

    public async Task<RefreshToken> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM RefreshTokens WHERE RefreshTokenId = @RefreshTokenId";
        const string sqlForUpdate = "SELECT * FROM RefreshTokens WHERE RefreshTokenId = @RefreshTokenId FOR UPDATE";
        return await _sqlConnection.QueryFirstOrDefaultAsync<RefreshToken>(forUpdate ? sqlForUpdate : sql,
                   new { RefreshTokenId = id }, _dbTransaction) ??
               throw new InvalidOperationException($"Refresh token with Id {id} not found");
    }

    public async Task<IEnumerable<RefreshToken>> GetByParams(Dictionary<string, string> parameters)
    {
        var builder = new SqlBuilder();

        if (parameters.TryGetValue("refreshTokenId", out var refreshTokenIdString) &&
            int.TryParse(refreshTokenIdString, out var refreshTokenId))
        {
            builder.Where("refreshTokenId = @RefreshTokenId", new { RefreshTokenId = refreshTokenId });
        }

        if (parameters.TryGetValue("userId", out var userIdString) && int.TryParse(userIdString, out var userId))
        {
            builder.Where("UserId = @UserId", new { UserId = userId });
        }

        if (parameters.TryGetValue("token", out var token) && !string.IsNullOrEmpty(token))
        {
            builder.Where("Token LIKE @Token", new { Token = $"%{token}%" });
        }

        if (parameters.TryGetValue("created", out var createdString) &&
            DateTime.TryParse(createdString, out var created))
        {
            builder.Where("Created = @Created", new { Created = created });
        }

        if (parameters.TryGetValue("expires", out var expiresString) &&
            DateTime.TryParse(expiresString, out var expires))
        {
            builder.Where("Expires = @Expires", new { Expires = expires });
        }

        if (parameters.TryGetValue("revoked", out var revokedString) &&
            DateTime.TryParse(revokedString, out var revoked))
        {
            builder.Where("Revoked = @Revoked", new { Revoked = revoked });
        }

        if (parameters.TryGetValue("replacedByToken", out var replacedByToken) &&
            !string.IsNullOrEmpty(replacedByToken))
        {
            builder.Where("ReplacedByToken LIKE @ReplacedByToken", new { ReplacedByToken = $"%{replacedByToken}%" });
        }

        builder.OrderBy("UserId");

        const string sql = "SELECT * FROM RefreshTokens /**where**/ /**orderby**/";
        var template = builder.AddTemplate(sql);

        return await _sqlConnection.QueryAsync<RefreshToken>(template.RawSql, template.Parameters, _dbTransaction);
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

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
}