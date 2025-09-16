using CounterStrikeItemsApi.Application.DTOs.Reference.Subtype;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CounterStrikeItemsApi.API.Controllers.References
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubtypeController(IReferenceService<SubtypeDto, Subtype, SubtypeCreateDto, SubtypeUpdateDto> service)
                : BaseReferenceController<SubtypeDto, Subtype, SubtypeCreateDto, SubtypeUpdateDto>(service) { }
}
