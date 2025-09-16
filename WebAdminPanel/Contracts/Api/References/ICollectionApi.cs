using Refit;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Models.DTOs.Reference;
using WebAdminPanel.Models.DTOs.Reference.Collection;

namespace WebAdminPanel.Contracts.Api.References
{
    public interface ICollectionApi : IBaseReferenceApi<CollectionDto, CollectionCreateDto, CollectionUpdateDto>
    {
        [Get("/collection")]
        new Task<List<CollectionDto>> GetAll();

        [Get("/collection/{id}")]
        new Task<CollectionDto> Get(Guid id);

        [Post("/collection/create")]
        new Task<ApiStringResponse> Create([Body] CollectionCreateDto dto);

        [Put("/collection/update")]
        new Task Update([Body] CollectionUpdateDto dto);

        [Delete("/collection/delete/{id}")]
        new Task Delete(Guid id);
    }
}
