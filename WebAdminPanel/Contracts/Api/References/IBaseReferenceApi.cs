using Refit;
using WebAdminPanel.Models.DTOs;

namespace WebAdminPanel.Contracts.Api.References
{
    public interface IBaseReferenceApi { }
    public interface IBaseReferenceApi<TDto, TCreateDto, TUpdateDto>
    {
        [Get("")]
        Task<List<TDto>> GetAll();

        [Get("/{id}")]
        Task<TDto> Get(Guid id);

        [Post("/create")]
        Task<ApiStringResponse> Create([Body] TCreateDto dto);

        [Put("/update")]
        Task Update([Body] TUpdateDto dto);

        [Delete("/delete/{id}")]
        Task Delete(Guid id);
    }
}