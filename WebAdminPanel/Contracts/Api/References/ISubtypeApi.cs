using Refit;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Models.DTOs.Reference.Subtype;

namespace WebAdminPanel.Contracts.Api.References
{
    public interface ISubtypeApi : IBaseReferenceApi<SubtypeDto, SubtypeCreateDto, SubtypeUpdateDto>
    {
        [Get("/subtype")]
        new Task<List<SubtypeDto>> GetAll();

        [Get("/subtype/{id}")]
        new Task<SubtypeDto> Get(Guid id);

        [Post("/subtype/create")]
        new Task<ApiStringResponse> Create([Body] SubtypeCreateDto dto);

        [Put("/subtype/update")]
        new Task Update([Body] SubtypeUpdateDto dto);

        [Delete("/subtype/delete/{id}")]
        new Task Delete(Guid id);
    }
}
