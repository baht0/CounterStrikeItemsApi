using Refit;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Contracts.Api.References
{
    public interface IQualityApi : IBaseReferenceApi<ReferenceColorDto, ReferenceColorCreateDto, ReferenceColorUpdateDto>
    {
        [Get("/quality")]
        new Task<List<ReferenceColorDto>> GetAll();

        [Get("/quality/{id}")]
        new Task<ReferenceColorDto> Get(Guid id);

        [Post("/quality/create")]
        new Task<ApiStringResponse> Create([Body] ReferenceColorCreateDto dto);

        [Put("/quality/update")]
        new Task Update([Body] ReferenceColorUpdateDto dto);

        [Delete("/quality/delete/{id}")]
        new Task Delete(Guid id);
    }
}
