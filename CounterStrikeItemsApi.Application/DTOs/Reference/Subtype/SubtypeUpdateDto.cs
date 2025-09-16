namespace CounterStrikeItemsApi.Application.DTOs.Reference.Subtype
{
    public class SubtypeUpdateDto : ReferenceUpdateDto, IReferenceUpdateDto
    {
        public List<Guid> ItemTypeIds { get; set; } = [];
    }
}
