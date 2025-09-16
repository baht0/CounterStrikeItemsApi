using System.ComponentModel.DataAnnotations;

namespace CounterStrikeItemsApi.Domain.Models
{
    public class SteamUser
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string SteamId { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsBanned { get; set; } = false;
    }
}
