using AutoMapper;
using CounterStrikeItemsApi.Application.DTOs;
using CounterStrikeItemsApi.Application.DTOs.ItemCommons;
using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.Helpers;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using System.Linq.Expressions;
using System.Net;
using System.Web;

namespace CounterStrikeItemsApi.Application.Services
{
    public class ItemCommonService(
        IItemCommonRepository itemCommonRepository,
        IRepositoryFactory repositoryFactory,
        IMapper mapper) 
        : IItemCommonService
    {
        private readonly IItemCommonRepository _itemCommonRep = itemCommonRepository;
        private readonly IRepositoryFactory _repositoryFactory = repositoryFactory;
        private readonly IMapper _mapper = mapper;

        private Expression<Func<ItemCommon, object>>[] _includes =>
        [
            x => x.Collection!,
            x => x.ProfessionalPlayer!,
            x => x.Team!,
            x => x.Tournament!,
            x => x.Type!,
            x => x.Subtype!,
            x => x.Items,
            x => x.FoundsAsItem
        ];
        private (Expression<Func<ItemCommon, object>>, Expression<Func<object, object>>)[] _thenIncludes =>
        [
            (x => x.Collection!, i => ((Collection)i).Type),
            (x => x.Items, i => ((Item)i).Category!),
            (x => x.Items, i => ((Item)i).Exterior!),
            (x => x.Items, i => ((Item)i).Quality!),
            (x => x.Items, i => ((Item)i).GraffitiColor!),
            (x => x.FoundsAsItem, f => ((Found)f).Container)
        ];

        public async Task<PagedResult<ItemCommonFilteredDto>> GetPaginatedResultAsync(ItemCommonFilterQuery query)
        {
            var filter = PredicateBuilder.True<ItemCommon>();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                string decodedStr = HttpUtility.UrlDecode(query.Search);
                var lower = decodedStr.ToLower();

                filter = filter.And(i =>
                    i.Name.ToLower().Contains(lower) ||
                    i.Items.Any(item => item.Name.ToLower() == lower)
                );
            }
            if (query.Collections?.Where(x => x != null).Any() == true)
            {
                var list = query.Collections.Select(t => t.ToLower()).ToList();
                filter = filter.And(i => list.Contains(i.Collection!.Slug.ToLower()));
            }
            if (query.Types?.Where(x => x != null).Any() == true)
            {
                var list = query.Types.Select(t => t.ToLower()).ToList();
                filter = filter.And(i => list.Contains(i.Type!.Slug.ToLower()));
            }
            if (query.Subtypes?.Where(x => x != null).Any() == true)
            {
                var list = query.Subtypes.Select(t => t.ToLower()).ToList();
                filter = filter.And(i => list.Contains(i.Subtype!.Slug.ToLower()));
            }
            if (query.Qualities?.Where(x => x != null).Any() == true)
            {
                var list = query.Qualities.Select(r => r.ToLower()).ToList();
                filter = filter.And(i => list.Contains(i.Items.FirstOrDefault()!.Quality!.Slug.ToLower()));
            }

            var (itemCommons, total) = await _itemCommonRep.GetPaginatedAsync(filter, query.Page, query.PageSize);

            var itemDtos = _mapper.Map<List<ItemCommonFilteredDto>>(itemCommons);

            return new PagedResult<ItemCommonFilteredDto>
            {
                Rows = itemDtos,
                TotalRows = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }
        public async Task<ItemCommonDto?> GetBySlugAsync(string slug)
        {
            var entity = await _itemCommonRep.GetBySlugAsync(slug, _includes, _thenIncludes);
            return entity == null ? null : _mapper.Map<ItemCommonDto>(entity);
        }

        public async Task<List<ReferenceDto>> GetAllContainers()
        {
            Expression<Func<ItemCommon, bool>> predicate = x => x.Type!.Name == "Container";

            var entity = await _itemCommonRep.FindAsync(predicate, _includes, _thenIncludes);
            return _mapper.Map<List<ReferenceDto>>(entity);
        }

        public async Task<string> AddItemCommonAsync(ItemCommonCreateBody body)
        {
            // Проверка наличия связанных сущностей
            await EnsureEntityExistsAsync<Collection>(body.CollectionId, "Collection");
            await EnsureEntityExistsAsync<ProfessionalPlayer>(body.ProfessionalPlayerId, "ProfessionalPlayer");
            await EnsureEntityExistsAsync<Tournament>(body.TournamentId, "Tournament");
            await EnsureEntityExistsAsync<Team>(body.TeamId, "Team");
            await EnsureEntityExistsAsync<ItemType>(body.TypeId, "Type");
            await EnsureEntityExistsAsync<Subtype>(body.SubtypeId, "Subtype");

            var itemCommon = _mapper.Map<ItemCommon>(body);

            // После маппинга — подставляем нужные связи:
            foreach (var item in itemCommon.Items)
                item.ItemCommonId = itemCommon.Id;

            foreach (var found in itemCommon.FoundsAsItem)
                found.ItemCommonId = itemCommon.Id;

            await _itemCommonRep.AddAsync(itemCommon);
            await _itemCommonRep.SaveChangesAsync();

            return itemCommon.Slug;
        }
        public async Task UpdateItemCommonAsync(ItemCommonUpdateBody body)
        {
            Expression<Func<ItemCommon, object>>[] _includes =
            [
                x => x.Items,
                x => x.FoundsAsItem
            ];

            // 1. Проверка существования
            var existing = await _itemCommonRep.GetByIdAsync(body.Id, _includes)
                ?? throw new HttpException(HttpStatusCode.NotFound, $"ItemCommon with id '{body.Id}' not found");

            // 2. Валидация ссылок
            await EnsureEntityExistsAsync<Collection>(body.CollectionId, "Collection");
            await EnsureEntityExistsAsync<ProfessionalPlayer>(body.ProfessionalPlayerId, "ProfessionalPlayer");
            await EnsureEntityExistsAsync<Tournament>(body.TournamentId, "Tournament");
            await EnsureEntityExistsAsync<Team>(body.TeamId, "Team");
            await EnsureEntityExistsAsync<ItemType>(body.TypeId, "Type");
            await EnsureEntityExistsAsync<Subtype>(body.SubtypeId, "Subtype");

            // 3. Обновление основных полей
            _mapper.Map(body, existing); // маппим базовые поля, кроме вложенных

            // 4. Обновление Items
            var itemRep = _repositoryFactory.GetRepository<Item>();
            // Преобразуем DTO в словарь по Id
            var bodyItems = body.Items ?? [];
            var bodyItemsById = bodyItems.Where(x => x.Id != null).ToDictionary(i => i.Id!.Value, i => i);
            // Обновляем существующие Items
            foreach (var existingItem in existing.Items.ToList())
            {
                if (bodyItemsById.TryGetValue(existingItem.Id, out var updatedDto))
                {
                    _mapper.Map(updatedDto, existingItem); // обновляем поля
                    bodyItemsById.Remove(existingItem.Id); // помечаем как обработанный
                }
                else
                    itemRep.Delete(existingItem);
            }
            // Добавление новых элементов (тех, у которых нет Id)
            var newItemDtos = bodyItems.Where(x => x.Id is null).ToList();
            foreach (var newDto in newItemDtos)
            {
                var newItem = _mapper.Map<Item>(newDto);
                newItem.Id = Guid.NewGuid();
                newItem.ItemCommonId = existing.Id;
                await itemRep.AddAsync(newItem);
                existing.Items.Add(newItem);
            }

            // 5. Обновление FoundsAsItem
            var foundRep = _repositoryFactory.GetRepository<Found>();
            var bodyContainerIds = body.ContainerIds?.ToHashSet() ?? [];

            var currentFounds = existing.FoundsAsItem.ToList();
            var currentContainerIds = currentFounds.Select(f => f.ContainerId).ToHashSet();
            // Удалить те связи, которых больше нет
            foreach (var found in currentFounds)
            {
                if (!bodyContainerIds.Contains(found.ContainerId))
                    foundRep.Delete(found);
            }
            // Добавить недостающие
            foreach (var containerId in bodyContainerIds)
            {
                if (!currentContainerIds.Contains(containerId))
                {
                    var container = await _itemCommonRep.GetByIdAsync(containerId, [ic => ic.Type!]);
                    if (container?.Type?.Name != "Container")
                        throw new Exception($"Item with id {containerId} is not a container.");

                    var newFound = new Found
                    {
                        ItemCommonId = existing.Id,
                        ContainerId = containerId
                    };

                    existing.FoundsAsItem.Add(newFound);
                }
            }

            await _itemCommonRep.SaveChangesAsync();
        }
        public async Task DeleteItemCommonAsync(Guid id)
        {
            var existing = await _itemCommonRep.GetByIdAsync(id)
                ?? throw new HttpException(HttpStatusCode.NotFound, $"ItemCommon with id '{id}' not found");

            _itemCommonRep.Delete(existing);
            await _itemCommonRep.SaveChangesAsync();
        }

        private async Task EnsureEntityExistsAsync<T>(Guid? id, string entityName) where T : class
        {
            if (id is null)
                return;

            var repo = _repositoryFactory.GetRepository<T>();
            var exists = await repo.ExistsAsync(id.Value);

            if (!exists)
                throw new HttpException(HttpStatusCode.BadRequest, $"'{entityName}' with ID '{id}' does not exist");
        }
    }
}