using CounterStrikeItemsApi.Application.DTOs.Reference;

namespace CounterStrikeItemsApi.Application.DTOs.Reference.Collection
{
    public class CollectionUpdateDto : ReferenceUpdateDto, IReferenceUpdateDto
    {
        public Guid TypeId { get; set; }
    }
}
