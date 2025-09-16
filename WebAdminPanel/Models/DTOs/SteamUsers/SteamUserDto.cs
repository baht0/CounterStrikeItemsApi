namespace CounterStrikeItemsApi.Application.DTOs.SteamUsers
{
    public class SteamUserDto
    {
        public Guid Id { get; set; }
        public string SteamId { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsBanned { get; set; } = false;
    }
}
