using Refit;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Contracts.Api.References
{
    public interface IProfessionalPlayerApi : IBaseReferenceApi<ReferenceDto, ReferenceCreateDto, ReferenceUpdateDto>
    {
        [Get("/professionalplayer")]
        new Task<List<ReferenceDto>> GetAll();

        [Get("/professionalplayer/{id}")]
        new Task<ReferenceDto> Get(Guid id);

        [Post("/professionalplayer/create")]
        new Task<ApiStringResponse> Create([Body] ReferenceCreateDto dto);

        [Put("/professionalplayer/update")]
        new Task Update([Body] ReferenceUpdateDto dto);

        [Delete("/professionalplayer/delete/{id}")]
        new Task Delete(Guid id);
    }
}
