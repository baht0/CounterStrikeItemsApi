using WebAdminPanel.Models.DTOs.Items;

namespace WebAdminPanel.Models.DTOs.ItemCommons
{
    public class ItemCommonUpdateBody : ItemCommonCreateBody
    {
        public Guid Id { get; set; }
        public new ICollection<ItemUpdateDto> Items { get; set; } = [];
    }
}
