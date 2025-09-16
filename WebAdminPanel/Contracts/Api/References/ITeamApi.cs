using Refit;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Contracts.Api.References
{
    public interface ITeamApi : IBaseReferenceApi<ReferenceDto, ReferenceCreateDto, ReferenceUpdateDto>
    {
        [Get("/team")]
        new Task<List<ReferenceDto>> GetAll();

        [Get("/team/{id}")]
        new Task<ReferenceDto> Get(Guid id);

        [Post("/team/create")]
        new Task<ApiStringResponse> Create([Body] ReferenceCreateDto dto);

        [Put("/team/update")]
        new Task Update([Body] ReferenceUpdateDto dto);

        [Delete("/team/delete/{id}")]
        new Task Delete(Guid id);
    }
}
