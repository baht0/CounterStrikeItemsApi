using Refit;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Contracts.Api.References
{
    public interface IExteriorApi : IBaseReferenceApi<ReferenceDto, ReferenceCreateDto, ReferenceUpdateDto>
    {
        [Get("/exterior")]
        new Task<List<ReferenceDto>> GetAll();

        [Get("/exterior/{id}")]
        new Task<ReferenceDto> Get(Guid id);

        [Post("/exterior/create")]
        new Task<ApiStringResponse> Create([Body] ReferenceCreateDto dto);

        [Put("/exterior/update")]
        new Task Update([Body] ReferenceUpdateDto dto);

        [Delete("/exterior/delete/{id}")]
        new Task Delete(Guid id);
    }
}
