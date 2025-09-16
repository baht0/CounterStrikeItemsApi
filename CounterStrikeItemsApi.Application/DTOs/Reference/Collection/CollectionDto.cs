using CounterStrikeItemsApi.Application.DTOs.Reference;

namespace CounterStrikeItemsApi.Application.DTOs.Reference.Collection
{
    public class CollectionDto : ReferenceDto
    {
        public ReferenceDto Type { get; set; } = null!;
    }
}
