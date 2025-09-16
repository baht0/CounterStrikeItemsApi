using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Domain.Interfaces;

namespace CounterStrikeItemsApi.Application.Interfaces
{
    public interface IReferenceService<TDto, TEntity, TCreateDto, TUpdateDto>
        where TDto : ReferenceDto
        where TEntity : class, IReferenceEntity, new()
        where TUpdateDto : IReferenceUpdateDto
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(TCreateDto dto);
        Task UpdateAsync(TUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
