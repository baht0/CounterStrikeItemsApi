using Refit;
using WebAdminPanel.Models.DTOs.TypeSubtype;

namespace WebAdminPanel.Contracts.Api
{
    public interface ITypeSubtypeApi
    {
        [Get("/typesubtype")]
        Task<IEnumerable<TypeSubtypeDto>> GetAll();

        [Get("/typesubtype/type/{typeId}")]
        Task<IEnumerable<TypeSubtypeDto>> GetSubtypesForType(Guid typeId);

        [Get("/typesubtype/subtype/{subtypeId}")]
        Task<IEnumerable<TypeSubtypeDto>> GetTypesForSubtype(Guid subtypeId);

        [Post("/typesubtype/create")]
        Task Create(Guid typeId, Guid subtypeId);

        [Delete("/typesubtype/delete")] 
        Task Delete(Guid typeId, Guid subtypeId);
    }
}