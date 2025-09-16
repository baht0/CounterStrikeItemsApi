using AutoMapper;
using CounterStrikeItemsApi.Application.DTOs.Items;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Interfaces;
using CounterStrikeItemsApi.Domain.Models;

namespace CounterStrikeItemsApi.Application.Services
{
    public class ItemSteamIdUpdater(IRepository<Item> itemRepository, IMapper mapper) : IItemSteamIdUpdater
    {
        private readonly IRepository<Item> _itemRep = itemRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ItemWorkerUpdateDto>> GetUpdateItemsAsync()
        {
            var items = await _itemRep.FindAsync(x => x.ImageId == null || x.ItemNameId == null);
            return _mapper.Map<List<ItemWorkerUpdateDto>>(items);
        }

        public async Task UpdateItemsPartialAsync(IEnumerable<ItemWorkerUpdateDto> items, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items);

            var dtos = items.Where(x => x != null).ToList();
            if (dtos.Count == 0) return;

            // Собираем уникальные Id и подтягиваем все сущности за один запрос
            var ids = dtos.Select(d => d.Id).Distinct().ToList();

            // Предполагаем, что FindAsync принимает предикат и возвращает IEnumerable<Item>
            var entities = (await _itemRep.FindAsync(e => ids.Contains(e.Id))).ToDictionary(e => e.Id, e => e);

            foreach (var dto in dtos)
            {
                if (!entities.TryGetValue(dto.Id, out var entity))
                {
                    // не найден — пропускаем. Можно вместо этого логировать или бросать исключение.
                    continue;
                }

                var changed = false;
                if (dto.ImageId != null && entity.ImageId != dto.ImageId)
                {
                    entity.ImageId = dto.ImageId;
                    changed = true;
                }
                if (dto.ItemNameId.HasValue && entity.ItemNameId != dto.ItemNameId.Value)
                {
                    entity.ItemNameId = dto.ItemNameId.Value;
                    changed = true;
                }
                if (changed)
                    _itemRep.Update(entity);
            }
            await _itemRep.SaveChangesAsync();
        }
    }
}