using CounterStrikeItemsApi.Application.DTOs;
using CounterStrikeItemsApi.Application.DTOs.ItemCommons;
using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CounterStrikeItemsApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCommonController(IItemCommonService service) : ControllerBase
    {
        private readonly IItemCommonService _itemService = service;

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<ItemCommonFilteredDto>>> GetByFilter(
            [FromQuery] ItemCommonFilterQuery query)
        {
            var result = await _itemService.GetPaginatedResultAsync(query);
            return Ok(result);
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<ItemCommonDto>> GetBySlug(string slug)
        {
            var itemCommonDto = await _itemService.GetBySlugAsync(slug);
            if (itemCommonDto == null)
                return NotFound();

            return Ok(itemCommonDto);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("containers")]
        public async Task<ActionResult<List<ReferenceDto>>> GetContainers()
        {
            var result = await _itemService.GetAllContainers();
            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ItemCommonCreateBody body)
        {
            if (body == null)
                return BadRequest();

            var slug = await _itemService.AddItemCommonAsync(body);
            return Ok(new { slug });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ItemCommonUpdateBody body)
        {
            if (body == null)
                return BadRequest();

            await _itemService.UpdateItemCommonAsync(body);
            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _itemService.DeleteItemCommonAsync(id);
            return NoContent();
        }
    }
}
