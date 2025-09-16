using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CounterStrikeItemsApi.API.Controllers.References
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController(IReferenceService<ReferenceDto, Team, ReferenceCreateDto, ReferenceUpdateDto> service)
        : BaseReferenceController<ReferenceDto, Team, ReferenceCreateDto, ReferenceUpdateDto>(service)
    { }
}