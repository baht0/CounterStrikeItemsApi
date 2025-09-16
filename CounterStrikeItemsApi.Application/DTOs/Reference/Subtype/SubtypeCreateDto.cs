namespace CounterStrikeItemsApi.Application.DTOs.Reference.Subtype
{
    public class SubtypeCreateDto : ReferenceCreateDto
    {
        public List<Guid> ItemTypeIds { get; set; } = [];
    }
}
