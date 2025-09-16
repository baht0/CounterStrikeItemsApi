using Refit;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Contracts.Api.References
{
    public interface IGraffitiColorApi : IBaseReferenceApi<ReferenceColorDto, ReferenceColorCreateDto, ReferenceColorUpdateDto>
    {
        [Get("/graffiticolor")]
        new Task<List<ReferenceColorDto>> GetAll();

        [Get("/graffiticolor/{id}")]
        new Task<ReferenceColorDto> Get(Guid id);

        [Post("/graffiticolor/create")]
        new Task<ApiStringResponse> Create([Body] ReferenceColorCreateDto dto);

        [Put("/graffiticolor/update")]
        new Task Update([Body] ReferenceColorUpdateDto dto);

        [Delete("/graffiticolor/delete/{id}")]
        new Task Delete(Guid id);
    }
}
