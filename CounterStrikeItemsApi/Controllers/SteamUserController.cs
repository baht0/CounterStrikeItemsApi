using CounterStrikeItemsApi.Application.DTOs;
using CounterStrikeItemsApi.Application.DTOs.SteamUsers;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CounterStrikeItemsApi.API.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class SteamUserController(ISteamUserService userService) : ControllerBase
    {
        private readonly ISteamUserService _userService = userService;

        [HttpGet("users")]
        public async Task<ActionResult<PagedResult<SteamUser>>> GetByFilter(
            [FromQuery] SteamUserFilterQuery query)
        {
            var result = await _userService.GetUsers(query);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] SteamUserUpdate body)
        {
            if (body == null)
                return BadRequest();

            await _userService.UpdateExistUserAsync(body);
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
