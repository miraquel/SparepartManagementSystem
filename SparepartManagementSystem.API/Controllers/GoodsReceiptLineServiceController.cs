using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GoodsReceiptLineServiceController : ControllerBase
{
    private readonly IGoodsReceiptLineService _goodsReceiptLineService;
    public GoodsReceiptLineServiceController(IGoodsReceiptLineService goodsReceiptLineService)
    {
        _goodsReceiptLineService = goodsReceiptLineService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GoodsReceiptLineDto>>> GetAll()
    {
        var result = await _goodsReceiptLineService.GetAll();
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<GoodsReceiptLineDto>> Add([FromBody] GoodsReceiptLineDto dto)
    {
        var result = await _goodsReceiptLineService.Add(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<GoodsReceiptLineDto>> Delete(int id)
    {
        var result = await _goodsReceiptLineService.Delete(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GoodsReceiptLineDto>> GetById(int id)
    {
        var result = await _goodsReceiptLineService.GetById(id);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpPut]
    public async Task<ActionResult<GoodsReceiptLineDto>> Update([FromBody] GoodsReceiptLineDto dto)
    {
        var result = await _goodsReceiptLineService.Update(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpGet(nameof(GetByParams))]
    public async Task<ActionResult<IEnumerable<GoodsReceiptLineDto>>> GetByParams([FromBody] GoodsReceiptLineDto dto)
    {
        var result = await _goodsReceiptLineService.GetByParams(dto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
}