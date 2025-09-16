using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CounterStrikeItemsApi.API.Controllers.References
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionalPlayerController(IReferenceService<ReferenceDto, ProfessionalPlayer, ReferenceCreateDto, ReferenceUpdateDto> service)
        : BaseReferenceController<ReferenceDto, ProfessionalPlayer, ReferenceCreateDto, ReferenceUpdateDto>(service)
    { }
}