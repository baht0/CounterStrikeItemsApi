using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CounterStrikeItemsApi.API.Controllers.References
{
    [Route("api/[controller]")]
    [ApiController]
    public class QualityController(IReferenceService<ReferenceColorDto, Quality, ReferenceColorCreateDto, ReferenceColorUpdateDto> service)
        : BaseReferenceController<ReferenceColorDto, Quality, ReferenceColorCreateDto, ReferenceColorUpdateDto>(service)
    { }
}