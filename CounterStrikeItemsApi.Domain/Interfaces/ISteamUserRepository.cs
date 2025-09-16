using CounterStrikeItemsApi.Domain.Models;
using System.Linq.Expressions;

namespace CounterStrikeItemsApi.Domain.Interfaces
{
    public interface ISteamUserRepository : IRepository<SteamUser>
    {
        Task<(List<SteamUser>, int)> GetPaginatedAsync(
            Expression<Func<SteamUser, bool>> predicate, int page, int pageSize);
        Task<SteamUser?> GetBySteamIdAsync(string steamId);
    }
}
