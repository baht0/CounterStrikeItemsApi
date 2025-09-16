using CounterStrikeItemsApi.Application.DTOs;
using CounterStrikeItemsApi.Application.DTOs.ItemCommons;
using CounterStrikeItemsApi.Application.DTOs.Items;
using CounterStrikeItemsApi.Application.DTOs.Reference;

namespace CounterStrikeItemsApi.Application.Interfaces
{
    public interface IItemCommonService
    {
        Task<PagedResult<ItemCommonFilteredDto>> GetPaginatedResultAsync(ItemCommonFilterQuery query);
        Task<ItemCommonDto?> GetBySlugAsync(string slug);
        Task<List<ReferenceDto>> GetAllContainers();

        Task<string> AddItemCommonAsync(ItemCommonCreateBody body);
        Task UpdateItemCommonAsync(ItemCommonUpdateBody body);

        Task DeleteItemCommonAsync(Guid id);
    }
}
