using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.API.Permission;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers;

[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
public class RowLevelAccessServiceController : ControllerBase
{
    private readonly IRowLevelAccessService _rowLevelAccessService;
        
    public RowLevelAccessServiceController(IRowLevelAccessService rowLevelAccessService)
    {
        _rowLevelAccessService = rowLevelAccessService;
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RowLevelAccessActivity.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetAllRowLevelAccess()
    {
        var result = await _rowLevelAccessService.GetAllRowLevelAccess();
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
        
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RowLevelAccessActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRowLevelAccessById(int id)
    {
        var result = await _rowLevelAccessService.GetRowLevelAccessById(id);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
        
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RowLevelAccessActivity.Create }])]
    [HttpPost]
    public async Task<IActionResult> AddRowLevelAccess([FromBody] RowLevelAccessDto dto)
    {
        var result = await _rowLevelAccessService.AddRowLevelAccess(dto);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
        
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RowLevelAccessActivity.Update }])]
    [HttpPut]
    public async Task<IActionResult> UpdateRowLevelAccess([FromBody] RowLevelAccessDto dto)
    {
        var result = await _rowLevelAccessService.UpdateRowLevelAccess(dto);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
        
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RowLevelAccessActivity.Delete }])]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRowLevelAccess(int id)
    {
        var result = await _rowLevelAccessService.DeleteRowLevelAccess(id);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RowLevelAccessActivity.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetRowLevelAccessByParams([FromQuery] Dictionary<string, string> parameters)
    {
        var result = await _rowLevelAccessService.GetRowLevelAccessByParams(parameters);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RowLevelAccessActivity.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetRowLevelAccessByUserId([FromQuery] int userId)
    {
        var result = await _rowLevelAccessService.GetRowLevelAccessByUserId(userId);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RowLevelAccessActivity.Delete }])]
    [HttpPost]
    public async Task<IActionResult> BulkDeleteRowLevelAccess([FromBody] IEnumerable<int> ids)
    {
        var result = await _rowLevelAccessService.BulkDeleteRowLevelAccess(ids);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}