using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.API.Permission;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers;

[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
public class WorkOrderServiceController : ControllerBase
{
    private readonly IWorkOrderService _workOrderService;
    public WorkOrderServiceController(IWorkOrderService workOrderService)
    {
        _workOrderService = workOrderService;
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Read }])]
    [HttpPost]
    public async Task<IActionResult> AddWorkOrderHeader(WorkOrderHeaderDto dto)
    {
        var response = await _workOrderService.AddWorkOrderHeader(dto);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Read }])]
    [HttpPost]
    public async Task<IActionResult> AddWorkOrderHeaderWithLines(WorkOrderHeaderDto dto)
    {
        var response = await _workOrderService.AddWorkOrderHeaderWithLines(dto);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Update }])]
    [HttpPut]
    public async Task<IActionResult> UpdateWorkOrderHeader(WorkOrderHeaderDto dto)
    {
        var response = await _workOrderService.UpdateWorkOrderHeader(dto);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Delete }])]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWorkOrderHeader(int id)
    {
        var response = await _workOrderService.DeleteWorkOrderHeader(id);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWorkOrderHeaderById(int id)
    {
        var response = await _workOrderService.GetWorkOrderHeaderById(id);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetAllWorkOrderHeaderPagedList([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var response = await _workOrderService.GetAllWorkOrderHeaderPagedList(pageNumber, pageSize);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetWorkOrderHeaderByParamsPagedList([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] Dictionary<string, string> parameters)
    {
        var response = await _workOrderService.GetWorkOrderHeaderByParamsPagedList(pageNumber, pageSize, parameters);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Read }])]
    [HttpPost]
    public async Task<IActionResult> AddWorkOrderLine(WorkOrderLineDto dto)
    {
        var response = await _workOrderService.AddWorkOrderLine(dto);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Update }])]
    [HttpPut]
    public async Task<IActionResult> UpdateWorkOrderLine(WorkOrderLineDto dto)
    {
        var response = await _workOrderService.UpdateWorkOrderLine(dto);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Delete }])]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWorkOrderLine(int id)
    {
        var response = await _workOrderService.DeleteWorkOrderLine(id);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWorkOrderLineById(int id)
    {
        var response = await _workOrderService.GetWorkOrderLineById(id);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWorkOrderLineByWorkOrderHeaderId(int id)
    {
        var response = await _workOrderService.GetWorkOrderLineByWorkOrderHeaderId(id);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWorkOrderHeaderByIdWithLines(int id)
    {
        var response = await _workOrderService.GetWorkOrderHeaderByIdWithLines(id);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Read }])]
    [HttpPost]
    public async Task<IActionResult> AddItemRequisition(ItemRequisitionDto dto)
    {
        var response = await _workOrderService.AddItemRequisition(dto);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Update }])]
    [HttpPut]
    public async Task<IActionResult> UpdateItemRequisition(ItemRequisitionDto dto)
    {
        var response = await _workOrderService.UpdateItemRequisition(dto);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Delete }])]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteItemRequisition(int id)
    {
        var response = await _workOrderService.DeleteItemRequisition(id);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetItemRequisitionById(int id)
    {
        var response = await _workOrderService.GetItemRequisitionById(id);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Read }])]
    [HttpGet]
    public async Task<IActionResult> GetItemRequisitionByParams([FromQuery] Dictionary<string, string> parameters)
    {
        var response = await _workOrderService.GetItemRequisitionByParams(parameters);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    
    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.WorkOrderActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetItemRequisitionByWorkOrderLineId(int id)
    {
        var response = await _workOrderService.GetItemRequisitionByWorkOrderLineId(id);
        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
}