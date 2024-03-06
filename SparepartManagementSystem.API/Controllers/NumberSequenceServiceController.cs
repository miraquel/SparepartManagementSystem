using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.API.Permission;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NumberSequenceServiceController : ControllerBase
    {
        private readonly INumberSequenceService _numberSequenceService;

        public NumberSequenceServiceController(INumberSequenceService numberSequenceService)
        {
            _numberSequenceService = numberSequenceService;
        }

        [HttpPost]
        [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.NumberSequence.Create } })]
        public async Task<IActionResult> Add(NumberSequenceDto dto)
        {
            var result = await _numberSequenceService.Add(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete($"{{{nameof(id)}:int}}")]
        [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.NumberSequence.Delete } })]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _numberSequenceService.Delete(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet]
        [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.NumberSequence.Read } })]
        public async Task<IActionResult> GetAll()
        {
            var result = await _numberSequenceService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet($"{{{nameof(id)}:int}}")]
        [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.NumberSequence.Read } })]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _numberSequenceService.GetById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet(nameof(GetByParams))]
        [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.NumberSequence.Read } })]
        public async Task<IActionResult> GetByParams(NumberSequenceDto dto)
        {
            var result = await _numberSequenceService.GetByParams(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
