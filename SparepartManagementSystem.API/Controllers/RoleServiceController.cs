using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.API.Permission;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleServiceController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleServiceController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.Role.Read } })]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var result = await _roleService.GetAll();
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet($"{{{nameof(id)}:int}}")]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.Role.Read } })]
    public async Task<ActionResult<RoleDto>> GetById(int id)
    {
        var result = await _roleService.GetById(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet(nameof(GetByParams))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.Role.Read } })]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetByParams([FromBody] RoleDto dto)
    {
        var result = await _roleService.GetByParams(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.Role.Create } })]
    public async Task<ActionResult<RoleDto>> Add([FromBody] RoleDto dto)
    {
        var result = await _roleService.Add(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPut]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.Role.Update } })]
    public async Task<ActionResult<RoleDto>> Update([FromBody] RoleDto dto)
    {
        var result = await _roleService.Update(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpDelete($"{{{nameof(id)}:int}}")]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.Role.Delete } })]
    public async Task<ActionResult<RoleDto>> Delete(int id)
    {
        var result = await _roleService.Delete(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost(nameof(AddUser))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.Role.Create } })]
    public async Task<ActionResult<RoleDto>> AddUser([FromBody] UserRoleDto dto)
    {
        var result = await _roleService.AddUser(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpDelete(nameof(DeleteUser))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.Role.Delete } })]
    public async Task<ActionResult<RoleDto>> DeleteUser([FromQuery] UserRoleDto dto)
    {
        var result = await _roleService.DeleteUser(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet(nameof(GetAllWithUsers))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.Role.Read } })]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllWithUsers()
    {
        var result = await _roleService.GetAllWithUsers();
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet($"{nameof(GetByIdWithUsers)}/{{id:int}}")]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.Role.Read } })]
    public async Task<ActionResult<RoleDto>> GetByIdWithUsers(int id)
    {
        var result = await _roleService.GetByIdWithUsers(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
}