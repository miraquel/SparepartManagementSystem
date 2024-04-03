using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.API.Permission;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GoodsReceiptServiceController : ControllerBase
{
    private readonly IGoodsReceiptService _goodsReceiptService;
    public GoodsReceiptServiceController(IGoodsReceiptService goodsReceiptService)
    {
        _goodsReceiptService = goodsReceiptService;
    }

    [HttpGet]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GoodsReceipt.Read } })]
    public async Task<IActionResult> GetAll()
    {
        var result = await _goodsReceiptService.GetAll();
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("{id:int}")]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GoodsReceipt.Read } })]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _goodsReceiptService.GetById(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpPost]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GoodsReceipt.Create } })]
    public async Task<IActionResult> Add([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptService.Add(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpPut]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GoodsReceipt.Update } })]
    public async Task<IActionResult> Update([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptService.Update(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpDelete("{id:int}")]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GoodsReceipt.Delete } })]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _goodsReceiptService.Delete(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpGet("[action]")]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GoodsReceipt.Read } })]
    public async Task<IActionResult> GetByParams([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptService.GetByParams(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpGet("[action]")]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GoodsReceipt.Read } })]
    public async Task<IActionResult> GetByParamsPagedList([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptService.GetByParamsPagedList(pageNumber, pageSize, dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpPost("[action]")]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GoodsReceipt.Create } })]
    public async Task<IActionResult> AddWithLines([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptService.AddWithLines(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpGet("[action]/{id:int}")]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GoodsReceipt.Read } })]
    public async Task<IActionResult> GetByIdWithLines(int id)
    {
        var result = await _goodsReceiptService.GetByIdWithLines(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost(nameof(PostToAx))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GoodsReceipt.Process } })]
    public async Task<IActionResult> PostToAx([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptService.PostToAx(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpPut("[action]")]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GoodsReceipt.Update } })]
    public async Task<IActionResult> UpdateWithLines([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptService.UpdateWithLines(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
}