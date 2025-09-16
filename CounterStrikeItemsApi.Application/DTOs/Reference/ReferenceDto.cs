namespace CounterStrikeItemsApi.Application.DTOs.Reference
{
    public class ReferenceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
    }
}
