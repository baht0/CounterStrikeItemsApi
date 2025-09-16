namespace CounterStrikeItemsApi.Application.DTOs.Reference.ItemType
{
    public class ItemTypeDto : ReferenceDto
    {
        public List<ReferenceDto> Subtypes { get; set; } = [];
    }
}
