using AutoMapper;
using CounterStrikeItemsApi.Application.DTOs.Reference.ItemType;
using CounterStrikeItemsApi.Domain.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using System.Linq.Expressions;

namespace CounterStrikeItemsApi.Application.Services.References
{
    public class ItemTypeService(IExtendedRepository<ItemType> repository, IMapper mapper, ReferenceCacheService cache) 
        : ReferenceService<ItemTypeDto, ItemType, ItemTypeCreateDto, ItemTypeUpdateDto>(repository, mapper, cache)
    {
        protected override Expression<Func<ItemType, object>>[]? Includes => [x => x.Subtypes];

        protected override (Expression<Func<ItemType, object>> Include, Expression<Func<object, object>> ThenInclude)[]? ThenIncludes =>
            [
                (x => x.Subtypes, i => ((ItemTypeSubtype)i).Subtype!)
            ];
    }
}
