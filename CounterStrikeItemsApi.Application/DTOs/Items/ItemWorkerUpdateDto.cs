namespace CounterStrikeItemsApi.Application.DTOs.Items
{
    public class ItemWorkerUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageId { get; set; }
        public int? ItemNameId { get; set; }

        public Guid ItemCommonId { get; set; }
        public Guid? ExteriorId { get; set; }
        public Guid? QualityId { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
