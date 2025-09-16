using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CounterStrikeItemsApi.Domain.Models
{
    public class ItemOrdersHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public long? HighestBuyOrder { get; set; }
        public long? LowestSellOrder { get; set; }
        public DateTime? CreatedAt { get; set; }

        //связь
        public Guid ItemId { get; set; }
        public Item Item { get; set; } = null!;
    }
}