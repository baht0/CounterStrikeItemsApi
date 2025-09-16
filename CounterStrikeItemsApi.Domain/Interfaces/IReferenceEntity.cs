namespace CounterStrikeItemsApi.Domain.Interfaces
{
    public interface IReferenceEntity
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Slug { get; set; }
    }
}
