using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GoodsReceiptHeaderServiceController : ControllerBase
{
    private readonly IGoodsReceiptHeaderService _goodsReceiptHeaderService;
    public GoodsReceiptHeaderServiceController(IGoodsReceiptHeaderService goodsReceiptHeaderService)
    {
        _goodsReceiptHeaderService = goodsReceiptHeaderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _goodsReceiptHeaderService.GetAll();
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _goodsReceiptHeaderService.GetById(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptHeaderService.Add(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptHeaderService.Update(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _goodsReceiptHeaderService.Delete(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetByParams([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptHeaderService.GetByParams(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetByParamsPagedList([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptHeaderService.GetByParamsPagedList(pageNumber, pageSize, dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> AddWithLines([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptHeaderService.AddWithLines(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> GetByIdWithLines(int id)
    {
        var result = await _goodsReceiptHeaderService.GetByIdWithLines(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost(nameof(PostToAx))]
    public async Task<IActionResult> PostToAx([FromBody] GoodsReceiptHeaderDto dto)
    {
        var result = await _goodsReceiptHeaderService.PostToAx(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
}