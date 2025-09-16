namespace CounterStrikeItemsApi.Application.DTOs.Reference.ItemType
{
    public class ItemTypeUpdateDto : ReferenceUpdateDto, IReferenceUpdateDto
    {
        public List<Guid> SubtypeIds { get; set; } = [];
    }
}
