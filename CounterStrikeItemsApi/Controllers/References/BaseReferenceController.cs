using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CounterStrikeItemsApi.API.Controllers.References
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseReferenceController<TDto, TEntity, TCreateDto, TUpdateDto>(IReferenceService<TDto, TEntity, TCreateDto, TUpdateDto> service) : ControllerBase
        where TDto : ReferenceDto
        where TEntity : class, IReferenceEntity, new()
        where TUpdateDto : IReferenceUpdateDto
    {
        private readonly IReferenceService<TDto, TEntity, TCreateDto, TUpdateDto> _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] TCreateDto dto)
        {
            var guid = await _service.CreateAsync(dto);
            return Ok(new { guid });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] TUpdateDto dto)
        {
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
