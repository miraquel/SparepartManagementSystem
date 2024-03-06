using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.API.Permission;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionServiceController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionServiceController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet("GetAllPermissionTypes")]
        [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.RolePermission.Read } })]
        public IActionResult GetAllPermissionTypes()
        {
            var result = _permissionService.GetAllPermissionTypes(); 

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetAllModules")]
        [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.RolePermission.Read } })]
        public IActionResult GetAllModules()
        {
            var result = _permissionService.GetAllModules();

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetByRoleId/{roleId}")]
        [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.RolePermission.Read } })]
        public async Task<IActionResult> GetByRoleId(int roleId)
        {
            var result = await _permissionService.GetByRoleId(roleId);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.RolePermission.Create } })]
        public async Task<IActionResult> Add(PermissionDto dto)
        {
            var result = await _permissionService.Add(dto);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("{id:int}")]
        [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.RolePermission.Delete } })]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _permissionService.Delete(id);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpGet]
        [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.RolePermission.Read } })]
        public async Task<IActionResult> GetAll()
        {
            var result = await _permissionService.GetAll();

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("{id:int}")]
        [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.RolePermission.Read } })]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _permissionService.GetById(id);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpGet(nameof(GetByParams))]
        [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.RolePermission.Read } })]
        public async Task<IActionResult> GetByParams(PermissionDto dto)
        {
            var result = await _permissionService.GetByParams(dto);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }
    }
}
