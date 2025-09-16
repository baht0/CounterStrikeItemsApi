using Refit;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Models.DTOs.ItemCommons;
using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Contracts.Api
{
    public interface IItemCommonApi
    {
        [Get("/itemcommon/search")]
        [QueryUriFormat(UriFormat.Unescaped)]
        Task<PagedResult<ItemCommonFilteredDto>> GetSearch(
            [Query] ItemCommonFilterQuery? query = null);

        [Get("/itemcommon/{slug}")]
        Task<ItemCommonDto> GetBySlug(string slug);

        [Get("/itemcommon/containers")]
        Task<List<ReferenceDto>> GetContainers();

        [Post("/itemcommon/create")]
        Task<ApiStringResponse> Create([Body] ItemCommonCreateBody body);

        [Put("/itemcommon/update")]
        Task Update([Body] ItemCommonUpdateBody body);

        [Delete("/itemcommon/delete/{id}")]
        Task Delete(Guid id);
    }
}