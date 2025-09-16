namespace CounterStrikeItemsApi.Application.DTOs.SteamUsers
{
    public class SteamUserFilterQuery
    {
        public string? Nickname { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}
