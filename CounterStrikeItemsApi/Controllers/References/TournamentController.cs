using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CounterStrikeItemsApi.API.Controllers.References
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController(IReferenceService<ReferenceDto, Tournament, ReferenceCreateDto, ReferenceUpdateDto> service)
        : BaseReferenceController<ReferenceDto, Tournament, ReferenceCreateDto, ReferenceUpdateDto>(service)
    { }
}