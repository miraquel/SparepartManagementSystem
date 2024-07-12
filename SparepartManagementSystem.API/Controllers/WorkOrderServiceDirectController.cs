using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers;

[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[Authorize]
[ApiController]
public class WorkOrderServiceDirectController : ControllerBase
{
    private readonly IWorkOrderServiceDirect _workOrderServiceDirect;

    public WorkOrderServiceDirectController(IWorkOrderServiceDirect workOrderServiceDirect)
    {
        _workOrderServiceDirect = workOrderServiceDirect;
    }
        
    // GET: api/WorkOrderServiceDirect/GetWorkOrderHeader?agsEamWoId=string
    [MapToApiVersion(1.0)]
    [HttpGet]
    public async Task<IActionResult> GetWorkOrderHeader(string agsEamWoId)
    {
        var response = await _workOrderServiceDirect.GetWorkOrderHeader(agsEamWoId);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
        
    // GET: api/WorkOrderServiceDirect/GetWorkOrderHeaderPagedList?pageNumber=int&pageSize=int&dto=WorkOrderHeaderDto
    [MapToApiVersion(1.0)]
    [HttpGet]
    public async Task<IActionResult> GetWorkOrderHeaderPagedList(int pageNumber, int pageSize, [FromQuery] WorkOrderHeaderDto dto)
    {
        var response = await _workOrderServiceDirect.GetWorkOrderHeaderPagedList(pageNumber, pageSize, dto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
        
    // POST: api/WorkOrderServiceDirect/AddWorkOrderLine
    [MapToApiVersion(1.0)]
    [HttpPost]
    public async Task<IActionResult> AddWorkOrderLine(WorkOrderLineDto dto)
    {
        var response = await _workOrderServiceDirect.AddWorkOrderLine(dto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
        
    // PUT: api/WorkOrderServiceDirect/UpdateWorkOrderLine
    [MapToApiVersion(1.0)]
    [HttpPut]
    public async Task<IActionResult> UpdateWorkOrderLine(WorkOrderLineDto dto)
    {
        var response = await _workOrderServiceDirect.UpdateWorkOrderLine(dto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
        
    // GET: api/WorkOrderServiceDirect/GetWorkOrderLine?agsEamWoId=string&line=int
    [MapToApiVersion(1.0)]
    [HttpGet]
    public async Task<IActionResult> GetWorkOrderLine(string agsEamWoId, int line)
    {
        var response = await _workOrderServiceDirect.GetWorkOrderLine(agsEamWoId, line);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
        
    // GET: api/WorkOrderServiceDirect/GetWorkOrderLineList?agsEamWoId=string
    [MapToApiVersion(1.0)]
    [HttpGet]
    public async Task<IActionResult> GetWorkOrderLineList(string agsEamWoId)
    {
        var response = await _workOrderServiceDirect.GetWorkOrderLineList(agsEamWoId);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    // POST: api/WorkOrderServiceDirect/CloseWorkOrderLineAndPostInventJournal
    [MapToApiVersion(1.0)]
    [HttpPost]
    public async Task<IActionResult> CloseWorkOrderLineAndPostInventJournal(WorkOrderLineDto dto)
    {
        var response = await _workOrderServiceDirect.CloseWorkOrderLineAndPostInventJournal(dto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
        
    // POST: api/WorkOrderServiceDirect/AddItemRequisition
    [MapToApiVersion(1.0)]
    [HttpPost]
    public async Task<IActionResult> AddItemRequisition(InventReqDto dto)
    {
        var response = await _workOrderServiceDirect.AddItemRequisition(dto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
        
    // PUT: api/WorkOrderServiceDirect/UpdateItemRequisition
    [MapToApiVersion(1.0)]
    [HttpPut]
    public async Task<IActionResult> UpdateItemRequisition(InventReqDto dto)
    {
        var response = await _workOrderServiceDirect.UpdateItemRequisition(dto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
        
    // DELETE: api/WorkOrderServiceDirect/DeleteItemRequisition
    [MapToApiVersion(1.0)]
    [HttpDelete]
    public async Task<IActionResult> DeleteItemRequisition([FromQuery] InventReqDto dto)
    {
        var response = await _workOrderServiceDirect.DeleteItemRequisition(dto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    // DELETE: api/WorkOrderServiceDirect/DeleteItemRequisitionWithListOfRecId
    [MapToApiVersion(1.0)]
    [HttpDelete]
    public async Task<IActionResult> DeleteItemRequisitionWithListOfRecId([FromQuery] IEnumerable<long> recIds)
    {
        var response = await _workOrderServiceDirect.DeleteItemRequisitionWithListOfRecId(recIds);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
        
    // GET: api/WorkOrderServiceDirect/GetItemRequisition?dto=InventReqDto
    [MapToApiVersion(1.0)]
    [HttpGet]
    public async Task<IActionResult> GetItemRequisition([FromQuery] InventReqDto dto)
    {
        var response = await _workOrderServiceDirect.GetItemRequisition(dto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
        
    // GET: api/WorkOrderServiceDirect/GetItemRequisitionList?agsWORecId=long
    [MapToApiVersion(1.0)]
    [HttpGet]
    public async Task<IActionResult> GetItemRequisitionList(long agswoRecId)
    {
        var response = await _workOrderServiceDirect.GetItemRequisitionList(agswoRecId);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    // POST: api/WorkOrderServiceDirect/CreateInventJournalTable
    [MapToApiVersion(1.0)]
    [HttpPost]
    public async Task<IActionResult> CreateInventJournalTable(WorkOrderLineDto dto)
    {
        var response = await _workOrderServiceDirect.CreateInventJournalTable(dto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}