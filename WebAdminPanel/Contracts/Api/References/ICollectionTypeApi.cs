using Refit;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Contracts.Api.References
{
    public interface ICollectionTypeApi : IBaseReferenceApi<ReferenceDto, ReferenceCreateDto, ReferenceUpdateDto>
    {
        [Get("/collectiontype")]
        new Task<List<ReferenceDto>> GetAll();

        [Get("/collectiontype/{id}")]
        new Task<ReferenceDto> Get(Guid id);

        [Post("/collectiontype/create")]
        new Task<ApiStringResponse> Create([Body] ReferenceCreateDto dto);

        [Put("/collectiontype/update")]
        new Task Update([Body] ReferenceUpdateDto dto);

        [Delete("/collectiontype/delete/{id}")]
        new Task Delete(Guid id);
    }
}
