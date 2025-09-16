using CounterStrikeItemsApi.Application.DTOs.Reference.Collection;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CounterStrikeItemsApi.API.Controllers.References
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollectionController(IReferenceService<CollectionDto, Collection, CollectionCreateDto, CollectionUpdateDto> service)
        : BaseReferenceController<CollectionDto, Collection, CollectionCreateDto, CollectionUpdateDto>(service) { }
}
