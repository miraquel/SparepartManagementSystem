using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Permission;

public class ClaimRequirementFilter : IAsyncAuthorizationFilter
{
    private readonly string[] _permissions;
    private readonly IPermissionService _permissionService;
    private readonly IUserService _userService;

    public ClaimRequirementFilter(IUserService userService, IPermissionService permissionService, string[] permissions)
    {
        _userService = userService;
        _permissionService = permissionService;
        _permissions = permissions;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userIdClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid");

        var hasPermission = await HasPermission(userIdClaim);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }

    private async Task<bool> HasPermission(Claim? userIdClaim)
    {
        var hasPermission = false;

        if (userIdClaim == null)
        {
            return hasPermission;
        }

        var userDto = await _userService.GetUserByIdWithRoles(int.Parse(userIdClaim.Value));

        if (userDto.Data == null)
        {
            return hasPermission;
        }

        if (userDto.Data.IsAdministrator)
        {
            hasPermission = true;
        }
        else
        {
            var roles = userDto.Data.Roles;
            var permissions = new List<PermissionDto>();

            foreach (var role in roles)
            {
                var permissionsDto = (await _permissionService.GetByRoleId(role.RoleId)).Data;

                if (permissionsDto != null)
                {
                    permissions.AddRange(permissionsDto);
                }
            }

            hasPermission = permissions.Exists(x => _permissions.Contains(x.PermissionName));
        }

        return hasPermission;
    }
}