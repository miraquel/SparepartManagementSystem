using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RowLevelAccessServiceController : ControllerBase
    {
        private readonly IRowLevelAccessService _rowLevelAccessService;
        
        public RowLevelAccessServiceController(IRowLevelAccessService rowLevelAccessService)
        {
            _rowLevelAccessService = rowLevelAccessService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _rowLevelAccessService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _rowLevelAccessService.GetById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RowLevelAccessDto dto)
        {
            var result = await _rowLevelAccessService.Add(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RowLevelAccessDto dto)
        {
            var result = await _rowLevelAccessService.Update(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _rowLevelAccessService.Delete(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByParams([FromBody] RowLevelAccessDto dto)
        {
            var result = await _rowLevelAccessService.GetByParams(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByUserId([FromQuery] int userId)
        {
            var result = await _rowLevelAccessService.GetByUserId(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        
        [HttpPost("[action]")]
        public async Task<IActionResult> BulkDelete([FromBody] IEnumerable<int> ids)
        {
            var result = await _rowLevelAccessService.BulkDelete(ids);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
