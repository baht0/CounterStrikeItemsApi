using AutoMapper;
using CounterStrikeItemsApi.Application.DTOs.Reference.Collection;
using CounterStrikeItemsApi.Domain.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using System.Linq.Expressions;

namespace CounterStrikeItemsApi.Application.Services.References
{
    public class CollectionService(IExtendedRepository<Collection> repository, IMapper mapper, ReferenceCacheService cache) 
        : ReferenceService<CollectionDto, Collection, CollectionCreateDto, CollectionUpdateDto>(repository, mapper, cache)
    {
        protected override Expression<Func<Collection, object>>[]? Includes => [x => x.Type];
    }
}
