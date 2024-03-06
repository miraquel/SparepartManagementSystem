using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.API.Permission;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserServiceController : ControllerBase
{
    private readonly IUserService _userService;

    public UserServiceController(IUserService userService)
    {
        _userService = userService;
    }

    // POST: api/UserService
    [HttpPost]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.User.Create } })]
    public async Task<ActionResult<UserDto>> Add([FromBody] UserDto dto)
    {
        var result = await _userService.Add(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    // DELETE: api/UserService/5
    [HttpDelete($"{{{nameof(id)}:int}}")]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.User.Delete } })]
    public async Task<ActionResult<UserDto>> Delete(int id)
    {
        var result = await _userService.Delete(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    // GET: api/UserService
    [HttpGet]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.User.Read } })]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var result = await _userService.GetAll();
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    // GET: api/UserService/GetAllWithRoles
    [HttpGet(nameof(GetAllWithRoles))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.User.Read } })]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllWithRoles()
    {
        var result = await _userService.GetAllWithRoles();
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    // GET: api/UserService/5
    [HttpGet($"{{{nameof(id)}:int}}")]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.User.Read } })]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var result = await _userService.GetById(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    // GET: api/UserService/GetByIdWithRoles/5
    [HttpGet($"{nameof(GetByIdWithRoles)}/{{userId:int}}")]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.User.Read } })]
    public async Task<ActionResult<UserDto>> GetByIdWithRoles(int userId)
    {
        var result = await _userService.GetByIdWithRoles(userId);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    // GET: api/UserService/GetByParams
    [HttpGet(nameof(GetByParams))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.User.Read } })]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetByParams([FromBody] UserDto dto)
    {
        var result = await _userService.GetByParams(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    // PUT: api/UserService/Update
    [HttpPut]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.User.Update } })]
    public async Task<ActionResult<UserDto>> Update([FromBody] UserDto dto)
    {
        var result = await _userService.Update(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost(nameof(LoginWithActiveDirectory))]
    [AllowAnonymous]
    public async Task<ActionResult<ServiceResponse<TokenDto>>> LoginWithActiveDirectory(UsernamePasswordDto usernamePassword)
    {
        var result = await _userService.LoginWithActiveDirectory(usernamePassword);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost(nameof(RefreshToken))]
    [AllowAnonymous]
    public async Task<ActionResult<ServiceResponse<TokenDto>>> RefreshToken([FromBody] TokenDto dto)
    {
        var result = await _userService.RefreshToken(dto.RefreshToken);
        if (result.Success)
            return Ok(result);
        return Unauthorized(result);
    }

    [HttpPost(nameof(RevokeToken))]
    [AllowAnonymous]
    public async Task<ActionResult<ServiceResponse<TokenDto>>> RevokeToken([FromBody] RefreshTokenDto dto)
    {
        var result = await _userService.RevokeToken(dto.Token);
        if (result.Success)
            return Ok(result);
        return Unauthorized(result);
    }

    [HttpPost(nameof(RevokeAllTokens))]
    [AllowAnonymous]
    public async Task<ActionResult<ServiceResponse<IEnumerable<RefreshTokenDto>>>> RevokeAllTokens([FromBody] RefreshTokenDto dto)
    {
        var result = await _userService.RevokeAllTokens(dto.UserId);
        if (result.Success)
            return Ok(result);
        return Unauthorized(result);
    }

    [HttpGet(nameof(GetUsersFromActiveDirectory))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.User.Read } })]
    public ActionResult<ServiceResponse<IEnumerable<UserDto>>> GetUsersFromActiveDirectory()
    {
        var result = _userService.GetUsersFromActiveDirectory();
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet(nameof(GetByUsernameWithRoles))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.User.Read } })]
    public async Task<IActionResult> GetByUsernameWithRoles(string username)
    {
        var result = await _userService.GetByUsernameWithRoles(username);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost(nameof(AddRole))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.User.Create } })]
    public async Task<IActionResult> AddRole(UserRoleDto dto)
    {
        var result = await _userService.AddRole(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpDelete(nameof(DeleteRole))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.User.Delete } })]
    public async Task<IActionResult> DeleteRole([FromQuery] UserRoleDto dto)
    {
        var result = await _userService.DeleteRole(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
}