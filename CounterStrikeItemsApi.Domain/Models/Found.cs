using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CounterStrikeItemsApi.Domain.Models
{
    public class Found
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid ItemCommonId { get; set; }
        public ItemCommon ItemCommon { get; set; } = null!;

        public Guid ContainerId { get; set; }
        public ItemCommon Container { get; set; } = null!;
    }
}