using System.Data;
using System.Data.SqlTypes;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.AspNetCore.Http;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;
using static System.String;

namespace SparepartManagementSystem.Repository.MySql;

internal partial class NumberSequenceRepositoryMySql : INumberSequenceRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDbConnection _sqlConnection;

    public NumberSequenceRepositoryMySql(IDbConnection sqlConnection, IDbTransaction dbTransaction,
        IHttpContextAccessor httpContextAccessor)
    {
        _sqlConnection = sqlConnection;
        _dbTransaction = dbTransaction;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> GetNextNumberByModule(string module)
    {
        const string sql = """
                           UPDATE NumberSequences
                           SET LastNumber = LastNumber + 1
                           WHERE Module = @module;
                           """;

        await _sqlConnection.ExecuteAsync(sql, new { module }, _dbTransaction);

        const string afterSql = """
                                SELECT * FROM NumberSequences
                                WHERE Module = @module;
                                """;

        var result = await _sqlConnection.QueryFirstOrDefaultAsync<NumberSequence>(afterSql, new { module }, _dbTransaction);
        var formatString = NumberSequenceRegex().Matches(result!.Format);
        return NumberSequenceRegex().Replace(result!.Format,
            result.LastNumber.ToString().PadLeft(formatString[0].Length, '0'));
    }

    public async Task Add(NumberSequence entity)
    {
        var currentDateTime = DateTime.Now;
        entity.CreatedBy = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.CreatedDateTime = currentDateTime;
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = currentDateTime;

        const string sql = """
                           INSERT INTO NumberSequences
                           (Name, Description, Module, Format, LastNumber, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES(@Name, @Description, @Module, @Format, @LastNumber, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
    }

    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM NumberSequences WHERE NumberSequenceId = @NumberSequenceId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { NumberSequenceId = id }, _dbTransaction);
    }

    public async Task<IEnumerable<NumberSequence>> GetAll()
    {
        const string sql = "SELECT * FROM NumberSequences";
        return await _sqlConnection.QueryAsync<NumberSequence>(sql, transaction: _dbTransaction);
    }

    public async Task<NumberSequence> GetById(int id)
    {
        const string sql = """
                           SELECT * FROM NumberSequences
                           WHERE NumberSequenceId = @NumberSequenceId
                           """;
        return await _sqlConnection.QueryFirstAsync<NumberSequence>(sql, new { NumberSequenceId = id }, _dbTransaction);
    }

    public async Task<IEnumerable<NumberSequence>> GetByParams(NumberSequence entity)
    {
        var builder = new SqlBuilder();

        if (entity.NumberSequenceId > 0)
            builder.Where("NumberSequenceId = @NumberSequenceId", new { entity.NumberSequenceId });
        if (!IsNullOrEmpty(entity.Name)) builder.Where("Name LIKE @Name", new { Name = $"%{entity.Name}%" });
        if (!IsNullOrEmpty(entity.Description))
            builder.Where("Description LIKE @Description", new { Description = $"%{entity.Description}%" });
        if (!IsNullOrEmpty(entity.Format)) builder.Where("Format LIKE @Format", new { Format = $"%{entity.Format}%" });
        if (entity.LastNumber > 0) builder.Where("LastNumber = @LastNumber", new { entity.LastNumber });
        if (!IsNullOrEmpty(entity.Module)) builder.Where("Module LIKE @Module", new { Module = $"%{entity.Module}%" });
        if (!IsNullOrEmpty(entity.CreatedBy))
            builder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{entity.CreatedBy}%" });
        if (entity.CreatedDateTime > SqlDateTime.MinValue.Value)
            builder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)", new { entity.CreatedDateTime });
        if (!IsNullOrEmpty(entity.ModifiedBy))
            builder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{entity.ModifiedBy}%" });
        if (entity.ModifiedDateTime > SqlDateTime.MinValue.Value)
            builder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { entity.ModifiedDateTime });

        var template = builder.AddTemplate("SELECT * FROM NumberSequences /**where**/");
        return await _sqlConnection.QueryAsync<NumberSequence>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(NumberSequence entity)
    {
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = DateTime.Now;

        var builder = new SqlBuilder();

        if (entity.NumberSequenceId > 0)
            builder.Where("NumberSequenceId = @NumberSequenceId", new { entity.NumberSequenceId });

        if (!IsNullOrEmpty(entity.Name)) builder.Set("Name = @Name", new { entity.Name });
        if (!IsNullOrEmpty(entity.Description))
            builder.Set("Description = @Description", new { entity.Description });
        if (!IsNullOrEmpty(entity.Format)) builder.Set("Format = @Format", new { entity.Format });
        if (entity.LastNumber > 0) builder.Set("LastNumber = @LastNumber", new { entity.LastNumber });
        if (!IsNullOrEmpty(entity.Module)) builder.Set("Module = @Module", new { entity.Module });
        if (!IsNullOrEmpty(entity.ModifiedBy))
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        if (entity.ModifiedDateTime > SqlDateTime.MinValue.Value)
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });

        const string sql = """
                           UPDATE NumberSequences
                           /**set**/
                           /**where**/
                           """;

        var template = builder.AddTemplate(sql);
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
    }
    public Task<int> GetLastInsertedId()
    {
        return _sqlConnection.ExecuteScalarAsync<int>("SELECT LAST_INSERT_ID()", transaction: _dbTransaction);
    }
    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;

    [GeneratedRegex("#+")]
    private static partial Regex NumberSequenceRegex();
}