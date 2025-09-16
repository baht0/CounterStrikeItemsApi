using System.Text.Json.Serialization;

namespace CounterStrikeItemsApi.Application.Models
{
    public class SteamPlayer
    {
        [JsonPropertyName("personaname")]
        public string Nickname { get; set; } = null!;

        [JsonPropertyName("avatarfull")]
        public string AvatarUrl { get; set; } = null!;
    }
}
