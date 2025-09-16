using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CounterStrikeItemsApi.API.Controllers.References
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionTypeController(IReferenceService<ReferenceDto, CollectionType, ReferenceCreateDto, ReferenceUpdateDto> service) 
        : BaseReferenceController<ReferenceDto, CollectionType, ReferenceCreateDto, ReferenceUpdateDto>(service)
    { }
}
