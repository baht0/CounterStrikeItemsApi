using CounterStrikeItemsApi.Application.DTOs.Items;

namespace CounterStrikeItemsApi.Application.Interfaces
{
    public interface IItemSteamIdUpdater
    {
        Task<IEnumerable<ItemWorkerUpdateDto>> GetUpdateItemsAsync();

        Task UpdateItemsPartialAsync(IEnumerable<ItemWorkerUpdateDto> items, CancellationToken cancellationToken = default);
    }
}
