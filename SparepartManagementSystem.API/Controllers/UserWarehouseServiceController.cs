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
public class UserWarehouseServiceController : ControllerBase
{
    private readonly IUserWarehouseService _userWarehouseService;

    public UserWarehouseServiceController(IUserWarehouseService userWarehouseService)
    {
        _userWarehouseService = userWarehouseService;
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserWarehouseActivity.Read }])]
    [HttpPost]
    public async Task<ActionResult<UserWarehouseDto>> AddUserWarehouse([FromBody] UserWarehouseDto dto)
    {
        var result = await _userWarehouseService.AddUserWarehouse(dto);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserWarehouseActivity.Delete }])]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<UserWarehouseDto>> DeleteUserWarehouse(int id)
    {
        var result = await _userWarehouseService.DeleteUserWarehouse(id);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
        
    // PUT: api/UserWarehouse/UpdateUserWarehouse
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserWarehouseActivity.Update }])]
    [HttpPut]
    public async Task<ActionResult<UserWarehouseDto>> UpdateUserWarehouse([FromBody] UserWarehouseDto dto)
    {
        var result = await _userWarehouseService.UpdateUserWarehouse(dto);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    // GET: api/UserWarehouse/GetAllUserWarehouse
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserWarehouseActivity.Read }])]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserWarehouseDto>>> GetAllUserWarehouse()
    {
        var result = await _userWarehouseService.GetAllUserWarehouse();
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    // GET: api/UserWarehouse/GetUserWarehouseById/5
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserWarehouseActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserWarehouseDto>> GetUserWarehouseById(int id)
    {
        var result = await _userWarehouseService.GetUserWarehouseById(id);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    // GET: api/UserWarehouse/GetUserWarehouseByParams
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserWarehouseActivity.Read }])]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserWarehouseDto>>> GetByParams([FromQuery] Dictionary<string, string> parameters)
    {
        var result = await _userWarehouseService.GetUserWarehouseByParams(parameters);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
        
    // GET: api/UserWarehouse/GetUserWarehouseByUserId/5
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserWarehouseActivity.Read }])]
    [HttpGet("{userId:int}")]
    public async Task<ActionResult<IEnumerable<UserWarehouseDto>>> GetUserWarehouseByUserId(int userId)
    {
        var result = await _userWarehouseService.GetUserWarehouseByUserId(userId);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    //[TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserWarehouseActivity.Read }])]
    [HttpGet("{userId:int}")]
    public async Task<ActionResult<UserWarehouseDto>> GetDefaultUserWarehouseByUserId(int userId)
    {
        var result = await _userWarehouseService.GetDefaultUserWarehouseByUserId(userId);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}