using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.API.Permission;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers;

[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[Authorize]
[ApiController]
public class VersionTrackerServiceController : ControllerBase
{
    private readonly IVersionTrackerService _versionTrackerService;

    public VersionTrackerServiceController(IVersionTrackerService versionTrackerService)
    {
        _versionTrackerService = versionTrackerService;
    }

    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.VersionTracker.Create }])]
    [HttpPost]
    public async Task<IActionResult> AddVersionTracker([FromBody] VersionTrackerDto dto)
    {
        var response = await _versionTrackerService.AddVersionTracker(dto);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.VersionTracker.Delete }])]
    [HttpDelete]
    public async Task<IActionResult> DeleteVersionTracker(int id)
    {
        var response = await _versionTrackerService.DeleteVersionTracker(id);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.VersionTracker.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetAllVersionTracker()
    {
        var response = await _versionTrackerService.GetAllVersionTracker();
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.VersionTracker.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetVersionTrackerById(int id)
    {
        var response = await _versionTrackerService.GetVersionTrackerById(id);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.VersionTracker.Update }])]
    [HttpPut]
    public async Task<IActionResult> UpdateVersionTracker([FromBody] VersionTrackerDto dto)
    {
        var response = await _versionTrackerService.UpdateVersionTracker(dto);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.VersionTracker.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetVersionTrackerByParams([FromQuery] Dictionary<string, string> parameters)
    {
        var response = await _versionTrackerService.GetVersionTrackerByParams(parameters);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
        
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.VersionTracker.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetLatestVersionTracker()
    {
        var response = await _versionTrackerService.GetLatestVersionTracker();
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> DownloadApkByVersion([FromQuery] string appVersion)
    {
        try
        {
            return await _versionTrackerService.DownloadApkByVersion(appVersion);
        }
        catch (Exception ex)
        {
            var serviceResponse = new ServiceResponse
            {
                Success = false,
                Message = ex.Message
            };

            return BadRequest(serviceResponse);
        }
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetVersionFeed()
    {
        try
        {
            var response = await _versionTrackerService.GetVersionFeed();
            return Content(response, "application/rss+xml");
        }
        catch (Exception ex)
        {
            var serviceResponse = new ServiceResponse
            {
                Success = false,
                Message = ex.Message
            };

            return BadRequest(serviceResponse);
        }
    }
}