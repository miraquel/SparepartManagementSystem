using System.Data;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Shared.DerivedClass;

namespace SparepartManagementSystem.Repository.MySql;

public class VersionTrackerRepositoryMySql : IVersionTrackerRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;

    public VersionTrackerRepositoryMySql(IDbTransaction dbTransaction, IDbConnection sqlConnection)
    {
        _dbTransaction = dbTransaction;
        _sqlConnection = sqlConnection;
    }

    public async Task Add(VersionTracker entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null)
    {
        onBeforeAdd?.Invoke(this, new AddEventArgs(entity));
        
        const string sql = """
                           INSERT INTO VersionTrackers (Version, Description, PhysicalLocation, PublishedDateTime, Sha1Checksum, CreatedDateTime, CreatedBy, ModifiedDateTime, ModifiedBy)
                            VALUES (@Version, @Description, @PhysicalLocation, @PublishedDateTime, @Sha1Checksum, @CreatedDateTime, @CreatedBy, @ModifiedDateTime, @ModifiedBy);
                           """;

        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
        entity.AcceptChanges();
        
        onAfterAdd?.Invoke(this, new AddEventArgs(entity));
    }

    public async Task Delete(int id)
    {
        const string sql = """
                           DELETE FROM VersionTrackers
                           WHERE VersionTrackerId = @VersionTrackerId;
                           """;

        _ = await _sqlConnection.ExecuteAsync(sql, new { VersionTrackerId = id }, _dbTransaction);
    }

    public async Task<IEnumerable<VersionTracker>> GetAll()
    {
        const string sql = "SELECT * FROM VersionTrackers ORDER BY Version DESC;";
        return await _sqlConnection.QueryAsync<VersionTracker>(sql, transaction: _dbTransaction);
    }

    public async Task<VersionTracker> GetById(int id, bool forUpdate = false)
    {
        const string sql = """
                           SELECT * FROM VersionTrackers
                           WHERE VersionTrackerId = @VersionTrackerId;
                           """;

        const string sqlForUpdate = """
                                    SELECT * FROM VersionTrackers
                                    WHERE VersionTrackerId = @VersionTrackerId
                                    FOR UPDATE;
                                    """;

        var result = await _sqlConnection.QueryFirstAsync<VersionTracker>(forUpdate ? sqlForUpdate : sql,
                   new { VersionTrackerId = id }, _dbTransaction) ??
               throw new InvalidOperationException($"Version tracker with Id {id} not found");
        result.AcceptChanges();
        
        return result;
    }

    public async Task<IEnumerable<VersionTracker>> GetByParams(Dictionary<string, string> parameters)
    {
        var builder = new SqlBuilder();

        if (parameters.TryGetValue("versionTrackerId", out var versionTrackerId) &&
            int.TryParse(versionTrackerId, out var versionTrackerIdValue))
        {
            builder.Where("VersionTrackerId = @VersionTrackerId", new { VersionTrackerId = versionTrackerIdValue });
        }

        if (parameters.TryGetValue("version", out var version) && !string.IsNullOrEmpty(version))
        {
            builder.Where("Version LIKE @Version", new { Version = $"%{version}%" });
        }

        if (parameters.TryGetValue("description", out var description) && !string.IsNullOrEmpty(description))
        {
            builder.Where("Description LIKE @Description", new { Description = $"%{description}%" });
        }

        if (parameters.TryGetValue("physicalLocation", out var physicalLocation) &&
            !string.IsNullOrEmpty(physicalLocation))
        {
            builder.Where("PhysicalLocation LIKE @PhysicalLocation",
                new { PhysicalLocation = $"%{physicalLocation}%" });
        }

        if (parameters.TryGetValue("publishedDateTime", out var publishedDateTime) &&
            DateTime.TryParse(publishedDateTime, out var publishedDateTimeValue))
        {
            builder.Where("PublishedDateTime = @PublishedDateTime", new { PublishedDateTime = publishedDateTimeValue });
        }

        if (parameters.TryGetValue("sha1Checksum", out var sha1Checksum) && !string.IsNullOrEmpty(sha1Checksum))
        {
            builder.Where("Sha1Checksum LIKE @Sha1Checksum", new { Sha1Checksum = $"%{sha1Checksum}%" });
        }

        if (parameters.TryGetValue("createdDateTime", out var createdDateTime) &&
            DateTime.TryParse(createdDateTime, out var createdDateTimeValue))
        {
            builder.Where("CreatedDateTime = @CreatedDateTime", new { CreatedDateTime = createdDateTimeValue });
        }

        if (parameters.TryGetValue("createdBy", out var createdBy) && !string.IsNullOrEmpty(createdBy))
        {
            builder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{createdBy}%" });
        }

        if (parameters.TryGetValue("modifiedDateTime", out var modifiedDateTime) &&
            DateTime.TryParse(modifiedDateTime, out var modifiedDateTimeValue))
        {
            builder.Where("ModifiedDateTime = @ModifiedDateTime", new { ModifiedDateTime = modifiedDateTimeValue });
        }

        if (parameters.TryGetValue("modifiedBy", out var modifiedBy) && !string.IsNullOrEmpty(modifiedBy))
        {
            builder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{modifiedBy}%" });
        }

        builder.OrderBy("Version DESC");

        var selector = builder.AddTemplate("SELECT * FROM VersionTrackers /**where**/ /**orderby**/;");

        return await _sqlConnection.QueryAsync<VersionTracker>(selector.RawSql, selector.Parameters, _dbTransaction);
    }

    public async Task Update(VersionTracker entity, EventHandler<BeforeUpdateEventArgs>? onBeforeUpdate = null,
        EventHandler<AfterUpdateEventArgs>? onAfterUpdate = null)
    {
        var builder = new CustomSqlBuilder();

        if (!entity.ValidateUpdate())
        {
            return;
        }

        if (entity.OriginalValue(nameof(VersionTracker.Version)) is not null && !Equals(entity.OriginalValue(nameof(entity.Version)), entity.Version))
        {
            builder.Set("Version = @Version", new { entity.Version });
        }

        if (entity.OriginalValue(nameof(VersionTracker.Description)) is not null && !Equals(entity.OriginalValue(nameof(VersionTracker.Description)), entity.Description))
        {
            builder.Set("Description = @Description", new { entity.Description });
        }

        if (entity.OriginalValue(nameof(VersionTracker.PhysicalLocation)) is not null && !Equals(entity.OriginalValue(nameof(VersionTracker.PhysicalLocation)), entity.PhysicalLocation))
        {
            builder.Set("PhysicalLocation = @PhysicalLocation", new { entity.PhysicalLocation });
        }

        if (entity.OriginalValue(nameof(VersionTracker.PublishedDateTime)) is not null && !Equals(entity.OriginalValue(nameof(VersionTracker.PublishedDateTime)), entity.PublishedDateTime))
        {
            builder.Set("PublishedDateTime = @PublishedDateTime", new { entity.PublishedDateTime });
        }

        if (entity.OriginalValue(nameof(VersionTracker.Sha1Checksum)) is not null && !Equals(entity.OriginalValue(nameof(VersionTracker.Sha1Checksum)), entity.Sha1Checksum))
        {
            builder.Set("Sha1Checksum = @Sha1Checksum", new { entity.Sha1Checksum });
        }
        
        builder.Where("VersionTrackerId = @VersionTrackerId", new { entity.VersionTrackerId });

        if (!builder.HasSet)
        {
            return;
        }
        
        onBeforeUpdate?.Invoke(this, new BeforeUpdateEventArgs(entity, builder));

        if (entity.OriginalValue(nameof(VersionTracker.ModifiedBy)) is not null && !Equals(entity.OriginalValue(nameof(VersionTracker.ModifiedBy)), entity.ModifiedBy))
        {
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (entity.OriginalValue(nameof(VersionTracker.ModifiedDateTime)) is not null && !Equals(entity.OriginalValue(nameof(VersionTracker.ModifiedDateTime)), entity.ModifiedDateTime))
        {
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }

        const string sql = """
                           UPDATE VersionTrackers
                           /**set**/
                           /**where**/
                           """;

        var template = builder.AddTemplate(sql);
        var rows = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
        if (rows == 0)
        {
            throw new InvalidOperationException($"Version tracker with Id {entity.VersionTrackerId} not found");
        }
        entity.AcceptChanges();
        
        onAfterUpdate?.Invoke(this, new AfterUpdateEventArgs(entity));
    }


    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;

    public async Task<VersionTracker> GetLatestVersionTracker()
    {
        const string sql = """
                           SELECT * FROM VersionTrackers
                           ORDER BY VersionTrackerId DESC
                           LIMIT 1;
                           """;

        return await _sqlConnection.QueryFirstOrDefaultAsync<VersionTracker>(sql, transaction: _dbTransaction) ??
               throw new Exception("No version tracker found");
    }

    public async Task<VersionTracker> GetByVersion(string version)
    {
        const string sql = """
                           SELECT * FROM VersionTrackers
                           WHERE Version = @Version;
                           """;

        return await _sqlConnection.QueryFirstOrDefaultAsync<VersionTracker>(sql, new { Version = version },
            _dbTransaction) ?? throw new Exception($"Version tracker with version {version} not found");
    }
}