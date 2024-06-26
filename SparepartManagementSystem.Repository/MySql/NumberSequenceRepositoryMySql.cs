using System.Data;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Shared.DerivedClass;
using SparepartManagementSystem.Shared.Helper;
using static System.String;

namespace SparepartManagementSystem.Repository.MySql;

internal class NumberSequenceRepositoryMySql : INumberSequenceRepository
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
                           SELECT * FROM NumberSequences
                           WHERE Module = @module;
                           """;

        var results = await _sqlConnection.QueryMultipleAsync(sql, new { module }, _dbTransaction);
        var numberSequenceResult = await results.ReadSingleAsync<NumberSequence>() ?? throw new InvalidOperationException($"Number sequence with module {module} not found");
        var formatString = RegexHelper.NumberSequenceRegex().Matches(numberSequenceResult.Format);
        return RegexHelper.NumberSequenceRegex().Replace(numberSequenceResult.Format,
            numberSequenceResult.LastNumber.ToString().PadLeft(formatString[0].Length, '0'));
    }

    public async Task Add(NumberSequence entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null)
    {
        onBeforeAdd?.Invoke(this, new AddEventArgs(entity));
        
        const string sql = """
                           INSERT INTO NumberSequences
                           (Name, Description, Module, Format, LastNumber, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES(@Name, @Description, @Module, @Format, @LastNumber, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
        entity.AcceptChanges();
        
        onAfterAdd?.Invoke(this, new AddEventArgs(entity));
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
        var result =
            await _sqlConnection.QueryFirstOrDefaultAsync<NumberSequence>(forUpdate ? sqlForUpdate : sql,
                new { NumberSequenceId = id }, _dbTransaction) ??
            throw new InvalidOperationException($"Number sequence with Id {id} not found");
        result.AcceptChanges();
        return result;
    }

    public async Task<IEnumerable<NumberSequence>> GetByParams(Dictionary<string, string> parameters)
    {
        var builder = new SqlBuilder();

        if (parameters.TryGetValue("numberSequenceId", out var numberSequenceIdString) &&
            int.TryParse(numberSequenceIdString, out var numberSequenceId))
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

        if (parameters.TryGetValue("lastNumber", out var lastNumberString) &&
            int.TryParse(lastNumberString, out var lastNumber))
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
            builder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)",
                new { ModifiedDateTime = modifiedDateTime });
        }

        var template = builder.AddTemplate("SELECT * FROM NumberSequences /**where**/");
        return await _sqlConnection.QueryAsync<NumberSequence>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(NumberSequence entity, EventHandler<UpdateEventArgs>? onBeforeUpdate = null, EventHandler<UpdateEventArgs>? onAfterUpdate = null)
    {
        var builder = new CustomSqlBuilder();
        
        onBeforeUpdate?.Invoke(this, new UpdateEventArgs(entity, builder));

        if (!entity.ValidateUpdate())
        {
            return;
        }

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

        if (!Equals(entity.OriginalValue(nameof(NumberSequence.ModifiedBy)), entity.ModifiedBy))
        {
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (!Equals(entity.OriginalValue(nameof(NumberSequence.ModifiedDateTime)), entity.ModifiedDateTime))
        {
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }

        builder.Where("NumberSequenceId = @NumberSequenceId", new { entity.NumberSequenceId });
        
        if (!builder.HasSet)
        {
            return;
        }

        const string sql = "UPDATE NumberSequences /**set**/ /**where**/";
        var template = builder.AddTemplate(sql);
        var rows = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
        if (rows == 0)
        {
            throw new InvalidOperationException($"Number sequence with Id {entity.NumberSequenceId} not found");
        }
        entity.AcceptChanges();
        
        onAfterUpdate?.Invoke(this, new UpdateEventArgs(entity, builder));
    }

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
}