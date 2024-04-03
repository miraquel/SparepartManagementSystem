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
public class WorkOrderServiceController : ControllerBase
{
    private readonly IWorkOrderService _workOrderService;
    public WorkOrderServiceController(IWorkOrderService workOrderService)
    {
        _workOrderService = workOrderService;
    }
    [HttpPost("AddWorkOrderHeader")]
    public async Task<IActionResult> AddWorkOrderHeader(WorkOrderHeaderDto dto)
    {
        var response = await _workOrderService.AddWorkOrderHeader(dto);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
    [HttpPut("UpdateWorkOrderHeader")]
    public async Task<IActionResult> UpdateWorkOrderHeader(WorkOrderHeaderDto dto)
    {
        var response = await _workOrderService.UpdateWorkOrderHeader(dto);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
    [HttpDelete("DeleteWorkOrderHeader/{id}")]
    public async Task<IActionResult> DeleteWorkOrderHeader(int id)
    {
        var response = await _workOrderService.DeleteWorkOrderHeader(id);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
    [HttpGet("GetWorkOrderHeaderById/{id}")]
    public async Task<IActionResult> GetWorkOrderHeaderById(int id)
    {
        var response = await _workOrderService.GetWorkOrderHeaderById(id);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
    [HttpGet("GetAllWorkOrderHeaderPagedList")]
    public async Task<IActionResult> GetAllWorkOrderHeaderPagedList([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var response = await _workOrderService.GetAllWorkOrderHeaderPagedList(pageNumber, pageSize);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
    [HttpGet("GetWorkOrderHeaderByParamsPagedList")]
    public async Task<IActionResult> GetWorkOrderHeaderByParamsPagedList([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] WorkOrderHeaderDto entity)
    {
        var response = await _workOrderService.GetWorkOrderHeaderByParamsPagedList(pageNumber, pageSize, entity);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
    [HttpPost("AddWorkOrderLine")]
    public async Task<IActionResult> AddWorkOrderLine(WorkOrderLineDto dto)
    {
        var response = await _workOrderService.AddWorkOrderLine(dto);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
    [HttpPut("UpdateWorkOrderLine")]
    public async Task<IActionResult> UpdateWorkOrderLine(WorkOrderLineDto dto)
    {
        var response = await _workOrderService.UpdateWorkOrderLine(dto);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
    [HttpDelete("DeleteWorkOrderLine/{id}")]
    public async Task<IActionResult> DeleteWorkOrderLine(int id)
    {
        var response = await _workOrderService.DeleteWorkOrderLine(id);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
    [HttpGet("GetWorkOrderLineById/{id}")]
    public async Task<IActionResult> GetWorkOrderLineById(int id)
    {
        var response = await _workOrderService.GetWorkOrderLineById(id);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
    [HttpGet("GetWorkOrderLineByWorkOrderHeaderId/{id}")]
    public async Task<IActionResult> GetWorkOrderLineByWorkOrderHeaderId(int id)
    {
        var response = await _workOrderService.GetWorkOrderLineByWorkOrderHeaderId(id);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
}