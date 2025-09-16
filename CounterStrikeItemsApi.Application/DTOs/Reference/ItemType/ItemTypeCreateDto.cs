namespace CounterStrikeItemsApi.Application.DTOs.Reference.ItemType
{
    public class ItemTypeCreateDto : ReferenceCreateDto
    {
        public List<Guid> SubtypeIds { get; set; } = [];
    }
}
