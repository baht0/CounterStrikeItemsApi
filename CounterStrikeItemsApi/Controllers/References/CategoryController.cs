using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CounterStrikeItemsApi.API.Controllers.References
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IReferenceService<ReferenceColorDto, Category, ReferenceColorCreateDto, ReferenceColorUpdateDto> service)
        : BaseReferenceController<ReferenceColorDto, Category, ReferenceColorCreateDto, ReferenceColorUpdateDto>(service)
    { }
}
