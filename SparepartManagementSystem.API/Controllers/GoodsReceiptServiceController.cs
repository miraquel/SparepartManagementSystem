using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.API.Permission;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers;

[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
public class GoodsReceiptServiceController : ControllerBase
{
    private readonly IGoodsReceiptService _goodsReceiptService;
    public GoodsReceiptServiceController(IGoodsReceiptService goodsReceiptService)
    {
        _goodsReceiptService = goodsReceiptService;
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.GoodsReceiptActivity.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetAllGoodsReceiptHeader()
    {
        var result = await _goodsReceiptService.GetAllGoodsReceiptHeader();
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.GoodsReceiptActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGoodsReceiptHeaderById(int id)
    {
        var result = await _goodsReceiptService.GetGoodsReceiptHeaderById(id);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.GoodsReceiptActivity.Create }])]
    [HttpPost]
    public async Task<IActionResult> AddGoodsReceiptHeader([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptService.AddGoodsReceiptHeader(dto);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.GoodsReceiptActivity.Update }])]
    [HttpPut]
    public async Task<IActionResult> UpdateGoodsReceiptHeader([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptService.UpdateGoodsReceiptHeader(dto);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.GoodsReceiptActivity.Delete }])]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteGoodsReceiptHeader(int id)
    {
        var result = await _goodsReceiptService.DeleteGoodsReceiptHeader(id);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.GoodsReceiptActivity.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetGoodsReceiptHeaderByParams([FromQuery] Dictionary<string, string> parameters)
    {
        var result = await _goodsReceiptService.GetGoodsReceiptHeaderByParams(parameters);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.GoodsReceiptActivity.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetGoodsReceiptHeaderByParamsPagedList([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] Dictionary<string, string> parameters)
    {
        var result = await _goodsReceiptService.GetByParamsPagedList(pageNumber, pageSize, parameters);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.GoodsReceiptActivity.Read }])]
    [HttpGet]
    public async Task<ActionResult<PagedList<GoodsReceiptHeaderDto>>> GetAllGoodsReceiptHeaderPagedList([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var result = await _goodsReceiptService.GetAllGoodsReceiptHeaderPagedList(pageNumber, pageSize);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.GoodsReceiptActivity.Create }])]
    [HttpPost]
    public async Task<IActionResult> AddGoodsReceiptHeaderWithLines([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptService.AddGoodsReceiptHeaderWithLines(dto);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.GoodsReceiptActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGoodsReceiptHeaderByIdWithLines(int id)
    {
        var result = await _goodsReceiptService.GetGoodsReceiptHeaderByIdWithLines(id);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.GoodsReceiptActivity.Process }])]
    [HttpPost]
    public async Task<IActionResult> PostToAx([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptService.PostToAx(dto);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.GoodsReceiptActivity.Update }])]
    [HttpPut]
    public async Task<IActionResult> UpdateGoodsReceiptHeaderWithLines([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptService.UpdateGoodsReceiptHeaderWithLines(dto);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}