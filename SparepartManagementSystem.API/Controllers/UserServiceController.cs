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
[ApiController]
[Authorize]
public class UserServiceController : ControllerBase
{
    private readonly IUserService _userService;

    public UserServiceController(IUserService userService)
    {
        _userService = userService;
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserActivity.Create }])]
    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] UserDto dto)
    {
        var result = await _userService.AddUser(dto);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserActivity.Delete }])]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<UserDto>> DeleteUser(int id)
    {
        var result = await _userService.DeleteUser(id);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserActivity.Read }])]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUser()
    {
        var result = await _userService.GetAllUser();
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserActivity.Read }])]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllWithRoles()
    {
        var result = await _userService.GetAllWithRoles();
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUserById(int id)
    {
        var result = await _userService.GetUserById(id);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserActivity.Read }])]
    [HttpGet("{userId:int}")]
    public async Task<ActionResult<UserDto>> GetUserByIdWithRoles(int userId)
    {
        var result = await _userService.GetUserByIdWithRoles(userId);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserActivity.Read }])]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUserByParams([FromQuery] Dictionary<string, string> parameters)
    {
        var result = await _userService.GetUserByParams(parameters);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserActivity.Update }])]
    [HttpPut]
    public async Task<ActionResult<UserDto>> UpdateUser([FromBody] UserDto dto)
    {
        var result = await _userService.UpdateUser(dto);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<TokenDto>>> LoginWithActiveDirectory(UsernamePasswordDto usernamePassword)
    {
        var result = await _userService.LoginWithActiveDirectory(usernamePassword);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<TokenDto>>> RefreshToken([FromBody] TokenDto dto)
    {
        var result = await _userService.RefreshToken(dto.RefreshToken);
        if (result.Success)
        {
            return Ok(result);
        }

        return Unauthorized(result);
    }

    [MapToApiVersion(1.0)]
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<TokenDto>>> RevokeToken([FromBody] RefreshTokenDto dto)
    {
        var result = await _userService.RevokeToken(dto.Token);
        if (result.Success)
        {
            return Ok(result);
        }

        return Unauthorized(result);
    }

    [MapToApiVersion(1.0)]
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<IEnumerable<RefreshTokenDto>>>> RevokeAllTokens([FromBody] RefreshTokenDto dto)
    {
        var result = await _userService.RevokeAllTokens(dto.UserId);
        if (result.Success)
        {
            return Ok(result);
        }

        return Unauthorized(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserActivity.Read }])]
    [HttpGet]
    public ActionResult<ServiceResponse<IEnumerable<UserDto>>> GetUsersFromActiveDirectory(string searchText = "")
    {
        var result = _userService.GetUsersFromActiveDirectory(searchText);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserActivity.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetUserByUsernameWithRoles(string username)
    {
        var result = await _userService.GetUserByUsernameWithRoles(username);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserActivity.Create }])]
    [HttpPost]
    public async Task<IActionResult> AddRoleToUser(UserRoleDto dto)
    {
        var result = await _userService.AddRoleToUser(dto);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserActivity.Delete }])]
    [HttpDelete]
    public async Task<IActionResult> DeleteRoleFromUser([FromQuery] UserRoleDto dto)
    {
        var result = await _userService.DeleteRoleFromUser(dto);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.UserActivity.Read }])]
    [HttpGet("{userId:int}")]
    public async Task<ActionResult<UserDto>> GetUserByIdWithUserWarehouse(int userId)
    {
        var result = await _userService.GetUserByIdWithUserWarehouse(userId);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}