using Refit;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Contracts.Api.References
{
    public interface ICategoryApi : IBaseReferenceApi<ReferenceColorDto, ReferenceColorCreateDto, ReferenceColorUpdateDto>
    {
        [Get("/category")]
        new Task<List<ReferenceColorDto>> GetAll();

        [Get("/category/{id}")]
        new Task<ReferenceColorDto> Get(Guid id);

        [Post("/category/create")]
        new Task<ApiStringResponse> Create([Body] ReferenceColorCreateDto dto);

        [Put("/category/update")]
        new Task Update([Body] ReferenceColorUpdateDto dto);

        [Delete("/category/delete/{id}")]
        new Task Delete(Guid id);
    }
}
