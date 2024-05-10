using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.API.Permission;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers.v1_0;

[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
[Authorize]
public class NumberSequenceServiceController : ControllerBase
{
    private readonly INumberSequenceService _numberSequenceService;

    public NumberSequenceServiceController(INumberSequenceService numberSequenceService)
    {
        _numberSequenceService = numberSequenceService;
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.NumberSequenceActivity.Create }])]
    [HttpPost]
    public async Task<IActionResult> AddNumberSequence(NumberSequenceDto dto)
    {
        var result = await _numberSequenceService.AddNumberSequence(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.NumberSequenceActivity.Delete }])]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteNumberSequence(int id)
    {
        var result = await _numberSequenceService.DeleteNumberSequence(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.NumberSequenceActivity.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetAllNumberSequence()
    {
        var result = await _numberSequenceService.GetAllNumberSequence();
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.NumberSequenceActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetNumberSequenceById(int id)
    {
        var result = await _numberSequenceService.GetNumberSequenceById(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.NumberSequenceActivity.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetNumberSequenceByParams([FromQuery] NumberSequenceDto dto)
    {
        var result = await _numberSequenceService.GetNumberSequenceByParams(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
}