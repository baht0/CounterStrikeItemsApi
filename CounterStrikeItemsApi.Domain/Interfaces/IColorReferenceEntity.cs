namespace CounterStrikeItemsApi.Domain.Interfaces
{
    public interface IColorReferenceEntity : IReferenceEntity
    {
        string? HexColor { get; set; }
    }
}
