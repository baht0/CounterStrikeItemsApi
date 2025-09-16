using AutoMapper;
using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.Helpers;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Interfaces;
using System.Linq.Expressions;
using System.Net;

namespace CounterStrikeItemsApi.Application.Services.References
{
    public class ReferenceService<TDto, TEntity, TCreateDto, TUpdateDto>(
        IExtendedRepository<TEntity> repository,
        IMapper mapper,
        ReferenceCacheService cache) : IReferenceService<TDto, TEntity, TCreateDto, TUpdateDto>
        where TDto : ReferenceDto
        where TEntity : class, IReferenceEntity, new()
        where TUpdateDto : IReferenceUpdateDto
    {
        private readonly IExtendedRepository<TEntity> _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ReferenceCacheService _cache = cache;

        protected virtual Expression<Func<TEntity, object>>[]? Includes => null;
        protected virtual (Expression<Func<TEntity, object>> Include, Expression<Func<object, object>> ThenInclude)[]? ThenIncludes => null;

        public virtual async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var cached = await _cache.GetAsync<IEnumerable<TDto>, TEntity>("all");
            if (cached != null)
                return cached;

            IEnumerable<TEntity> entities;
            if (Includes?.Length > 0 && ThenIncludes?.Length > 0)
                entities = await _repository.GetAllAsync(Includes, ThenIncludes);
            else if (Includes?.Length > 0)
                entities = await _repository.GetAllAsync(Includes);
            else
                entities = await _repository.GetAllAsync();

            var mapped = entities.OrderBy(x => x.Slug).Select(_mapper.Map<TDto>).ToList();

            await _cache.SetAsync<IEnumerable<TDto>, TEntity>("all", mapped);

            return mapped;
        }

        public virtual async Task<TDto?> GetByIdAsync(Guid id)
        {
            var cached = await _cache.GetAsync<TDto, TEntity>(id.ToString());
            if (cached != null)
                return cached;

            TEntity? entity;
            if (Includes?.Length > 0 && ThenIncludes?.Length > 0)
                entity = await _repository.GetByIdAsync(id, Includes, ThenIncludes);
            else if (Includes?.Length > 0)
                entity = await _repository.GetByIdAsync(id, Includes);
            else
                entity = await _repository.GetByIdAsync(id);

            if (entity == null) return null;

            var mapped = _mapper.Map<TDto>(entity);

            await _cache.SetAsync<TDto, TEntity>(id.ToString(), mapped);

            return mapped;
        }

        public virtual async Task<Guid> CreateAsync(TCreateDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return entity.Id;
        }

        public virtual async Task UpdateAsync(TUpdateDto dto)
        {
            var existing = (await _repository.FindAsync(e => e.Id == dto.Id)).FirstOrDefault()
                ?? throw new HttpException(HttpStatusCode.NotFound, "Such element not found!");

            _mapper.Map(dto, existing);
            existing.Slug = StringToSlug.Generate((dto as dynamic)?.Name);

            _repository.Update(existing);
            await _repository.SaveChangesAsync();
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var deleted = await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
            return deleted;
        }
    }
}
