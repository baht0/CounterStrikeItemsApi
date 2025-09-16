using System.ComponentModel.DataAnnotations;

namespace CounterStrikeItemsApi.Application.DTOs.SteamUsers
{
    public class SteamUserFilterQuery
    {
        public string? Nickname { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int Page { get; set; } = 1;
        [Range(10, 100)]
        public int PageSize { get; set; } = 25;
    }
}
