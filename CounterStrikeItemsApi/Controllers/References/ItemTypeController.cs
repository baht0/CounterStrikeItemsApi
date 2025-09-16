using CounterStrikeItemsApi.Application.DTOs.Reference.ItemType;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CounterStrikeItemsApi.API.Controllers.References
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemTypeController(IReferenceService<ItemTypeDto, ItemType, ItemTypeCreateDto, ItemTypeUpdateDto> service)
        : BaseReferenceController<ItemTypeDto, ItemType, ItemTypeCreateDto, ItemTypeUpdateDto>(service) { }
}
