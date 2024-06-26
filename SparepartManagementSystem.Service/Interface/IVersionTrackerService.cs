using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IVersionTrackerService
{
    Task<ServiceResponse> AddVersionTracker(VersionTrackerDto dto);
    Task<ServiceResponse> DeleteVersionTracker(int id);
    Task<ServiceResponse<IEnumerable<VersionTrackerDto>>> GetAllVersionTracker();
    Task<ServiceResponse<VersionTrackerDto>> GetVersionTrackerById(int id);
    Task<ServiceResponse> UpdateVersionTracker(VersionTrackerDto dto);
    Task<ServiceResponse<IEnumerable<VersionTrackerDto>>> GetVersionTrackerByParams(Dictionary<string, string> parameters);
    Task<ServiceResponse<VersionTrackerDto>> GetLatestVersionTracker();
    Task<ServiceResponse<VersionTrackerDto>> GetVersionTrackerByVersion(string version);
    Task<FileContentResult> DownloadApkByVersion(string version);
    Task<string> GetVersionFeed();
}