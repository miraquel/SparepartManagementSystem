using System.Data;
using System.Text.RegularExpressions;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;
using static System.String;

namespace SparepartManagementSystem.Repository.MySql;

internal partial class NumberSequenceRepositoryMySql : INumberSequenceRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;

    public NumberSequenceRepositoryMySql(IDbConnection sqlConnection, IDbTransaction dbTransaction)
    {
        _sqlConnection = sqlConnection;
        _dbTransaction = dbTransaction;
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
        return NumberSequenceRegex().Replace(result.Format,
            result.LastNumber.ToString().PadLeft(formatString[0].Length, '0'));
    }

    public async Task Add(NumberSequence entity)
    {
        const string sql = """
                           INSERT INTO NumberSequences
                           (Name, Description, Module, Format, LastNumber, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES(@Name, @Description, @Module, @Format, @LastNumber, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
        entity.AcceptChanges();
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

    public async Task<NumberSequence> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM NumberSequences WHERE NumberSequenceId = @NumberSequenceId";
        const string sqlForUpdate = "SELECT * FROM NumberSequences WHERE NumberSequenceId = @NumberSequenceId FOR UPDATE";
        var result = await _sqlConnection.QueryFirstAsync<NumberSequence>(forUpdate ? sqlForUpdate : sql, new { NumberSequenceId = id }, _dbTransaction);
        result.AcceptChanges();
        return result;
    }

    public async Task<IEnumerable<NumberSequence>> GetByParams(Dictionary<string, string> parameters)
    {
        var builder = new SqlBuilder();
        
        if (parameters.TryGetValue("numberSequenceId", out var numberSequenceIdString) && int.TryParse(numberSequenceIdString, out var numberSequenceId))
        {
            builder.Where("NumberSequenceId = @NumberSequenceId", new { NumberSequenceId = numberSequenceId });
        }

        if (parameters.TryGetValue("name", out var name) && !IsNullOrEmpty(name))
        {
            builder.Where("Name LIKE @Name", new { Name = $"%{name}%" });
        }

        if (parameters.TryGetValue("description", out var description) && !IsNullOrEmpty(description))
        {
            builder.Where("Description LIKE @Description", new { Description = $"%{description}%" });
        }

        if (parameters.TryGetValue("format", out var format) && !IsNullOrEmpty(format))
        {
            builder.Where("Format LIKE @Format", new { Format = $"%{format}%" });
        }

        if (parameters.TryGetValue("lastNumber", out var lastNumberString) && int.TryParse(lastNumberString, out var lastNumber))
        {
            builder.Where("LastNumber = @LastNumber", new { LastNumber = lastNumber });
        }

        if (parameters.TryGetValue("module", out var module) && !IsNullOrEmpty(module))
        {
            builder.Where("Module LIKE @Module", new { Module = $"%{module}%" });
        }

        if (parameters.TryGetValue("createdBy", out var createdBy) && !IsNullOrEmpty(createdBy))
        {
            builder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{createdBy}%" });
        }

        if (parameters.TryGetValue("createdDateTime", out var createdDateTimeString) && DateTime.TryParse(createdDateTimeString, out var createdDateTime))
        {
            builder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)", new { CreatedDateTime = createdDateTime });
        }

        if (parameters.TryGetValue("modifiedBy", out var modifiedBy) && !IsNullOrEmpty(modifiedBy))
        {
            builder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{modifiedBy}%" });
        }

        if (parameters.TryGetValue("modifiedDateTime", out var modifiedDateTimeString) && DateTime.TryParse(modifiedDateTimeString, out var modifiedDateTime))
        {
            builder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { ModifiedDateTime = modifiedDateTime });
        }

        var template = builder.AddTemplate("SELECT * FROM NumberSequences /**where**/");
        return await _sqlConnection.QueryAsync<NumberSequence>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(NumberSequence entity)
    {
        var builder = new SqlBuilder();

        if (!Equals(entity.OriginalValue(nameof(NumberSequence.Name)), entity.Name))
        {
            builder.Set("Name = @Name", new { entity.Name });
        }
        if (!Equals(entity.OriginalValue(nameof(NumberSequence.Description)), entity.Description))
        {
            builder.Set("Description = @Description", new { entity.Description });
        }
        if (!Equals(entity.OriginalValue(nameof(NumberSequence.Format)), entity.Format))
        {
            builder.Set("Format = @Format", new { entity.Format });
        }
        if (!Equals(entity.OriginalValue(nameof(NumberSequence.LastNumber)), entity.LastNumber))
        {
            builder.Set("LastNumber = @LastNumber", new { entity.LastNumber });
        }
        if (!Equals(entity.OriginalValue(nameof(NumberSequence.Module)), entity.Module))
        {
            builder.Set("Module = @Module", new { entity.Module });
        }
        if (!Equals(entity.OriginalValue(nameof(NumberSequence.CreatedBy)), entity.CreatedBy))
        {
            builder.Set("CreatedBy = @CreatedBy", new { entity.CreatedBy });
        }
        
        if (!Equals(entity.OriginalValue(nameof(NumberSequence.CreatedDateTime)), entity.CreatedDateTime))
        {
            builder.Set("CreatedDateTime = @CreatedDateTime", new { entity.CreatedDateTime });
        }
        
        builder.Where("NumberSequenceId = @NumberSequenceId", new { entity.NumberSequenceId });

        const string sql = "UPDATE NumberSequences /**set**/ /**where**/";
        var template = builder.AddTemplate(sql);
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
        entity.AcceptChanges();
    }
    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;

    [GeneratedRegex("#+")]
    private static partial Regex NumberSequenceRegex();
}