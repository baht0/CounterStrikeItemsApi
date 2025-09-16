using Refit;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Models.DTOs.Reference.ItemType;

namespace WebAdminPanel.Contracts.Api.References
{
    public interface IItemTypeApi : IBaseReferenceApi<ItemTypeDto, ItemTypeCreateDto, ItemTypeUpdateDto>
    {
        [Get("/itemtype")]
        new Task<List<ItemTypeDto>> GetAll();

        [Get("/itemtype/{id}")]
        new Task<ItemTypeDto> Get(Guid id);

        [Post("/itemtype/create")]
        new Task<ApiStringResponse> Create([Body] ItemTypeCreateDto dto);

        [Put("/itemtype/update")]
        new Task Update([Body] ItemTypeUpdateDto dto);

        [Delete("/itemtype/delete/{id}")]
        new Task Delete(Guid id);
    }
}
