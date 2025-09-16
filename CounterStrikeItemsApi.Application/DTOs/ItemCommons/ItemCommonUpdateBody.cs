using CounterStrikeItemsApi.Application.DTOs.Items;

namespace CounterStrikeItemsApi.Application.DTOs.ItemCommons
{
    public class ItemCommonUpdateBody : ItemCommonCreateBody
    {
        public Guid Id { get; set; }
        public new ICollection<ItemUpdateDto> Items { get; set; } = [];
    }
}
