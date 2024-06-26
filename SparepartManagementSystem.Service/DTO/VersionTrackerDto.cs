using System.Data.SqlTypes;

namespace SparepartManagementSystem.Service.DTO;

public class VersionTrackerDto
{
    public int VersionTrackerId { get; init; }
    public string Version { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string PhysicalLocation { get; init; } = string.Empty;
    public DateTime PublishedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    public string Sha1Checksum { get; init; } = string.Empty;
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    public string ModifiedBy { get; init; } = string.Empty;
    public DateTime ModifiedDateTime { get; init; } = SqlDateTime.MinValue.Value;
}