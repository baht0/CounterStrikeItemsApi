using Refit;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Contracts.Api.References
{
    public interface ITournamentApi : IBaseReferenceApi<ReferenceDto, ReferenceCreateDto, ReferenceUpdateDto>
    {
        [Get("/tournament")]
        new Task<List<ReferenceDto>> GetAll();

        [Get("/tournament/{id}")]
        new Task<ReferenceDto> Get(Guid id);

        [Post("/tournament/create")]
        new Task<ApiStringResponse> Create([Body] ReferenceCreateDto dto);

        [Put("/tournament/update")]
        new Task Update([Body] ReferenceUpdateDto dto);

        [Delete("/tournament/delete/{id}")]
        new Task Delete(Guid id);
    }
}
