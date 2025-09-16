using AutoMapper;
using CounterStrikeItemsApi.Application.DTOs.Reference.Subtype;
using CounterStrikeItemsApi.Domain.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using System.Linq.Expressions;

namespace CounterStrikeItemsApi.Application.Services.References
{
    public class SubtypeService(IExtendedRepository<Subtype> repository, IMapper mapper, ReferenceCacheService cache)
        : ReferenceService<SubtypeDto, Subtype, SubtypeCreateDto, SubtypeUpdateDto>(repository, mapper, cache)
    {
        protected override Expression<Func<Subtype, object>>[]? Includes => [x => x.ItemTypes];

        protected override (Expression<Func<Subtype, object>> Include, Expression<Func<object, object>> ThenInclude)[]? ThenIncludes =>
            [
                (x => x.ItemTypes, i => ((ItemTypeSubtype)i).ItemType!)
            ];
    }
}
