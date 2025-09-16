namespace CounterStrikeItemsApi.Application.DTOs.Reference.Subtype
{
    public class SubtypeDto : ReferenceDto
    {
        public List<ReferenceDto> ItemTypes { get; set; } = [];
    }
}
