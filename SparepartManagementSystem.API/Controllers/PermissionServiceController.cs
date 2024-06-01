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
public class PermissionServiceController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionServiceController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.PermissionActivity.Read }])]
    [HttpGet]
    public ActionResult<ServiceResponse<IEnumerable<PermissionDto>>> GetAllPermissionTypes()
    {
        var result = _permissionService.GetAllPermissionTypes(); 

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.PermissionActivity.Read }])]
    [HttpGet]
    public ActionResult<ServiceResponse<IEnumerable<PermissionDto>>> GetAllModules()
    {
        var result = _permissionService.GetAllModules();

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.PermissionActivity.Read }])]
    [HttpGet("{roleId:int}")]
    public async Task<ActionResult<ServiceResponse<IEnumerable<PermissionDto>>>> GetByRoleId(int roleId)
    {
        var result = await _permissionService.GetByRoleId(roleId);

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.PermissionActivity.Create }])]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse>> AddPermission(PermissionDto dto)
    {
        var result = await _permissionService.AddPermission(dto);

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.PermissionActivity.Delete }])]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ServiceResponse>> DeletePermission(int id)
    {
        var result = await _permissionService.DeletePermission(id);

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.PermissionActivity.Read }])]
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<IEnumerable<PermissionDto>>>> GetAllPermission()
    {
        var result = await _permissionService.GetAllPermission();

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.PermissionActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPermissionById(int id)
    {
        var result = await _permissionService.GetPermissionById(id);

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.PermissionActivity.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetPermissionByParams([FromQuery] Dictionary<string, string> parameters)
    {
        var result = await _permissionService.GetPermissionByParams(parameters);

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}