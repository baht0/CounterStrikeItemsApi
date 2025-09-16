using CounterStrikeItemsApi.Domain.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CounterStrikeItemsApi.Infrastructure.Repositories
{
    public class SteamUserRepository(AppDbContext context) : Repository<SteamUser>(context), ISteamUserRepository
    {
        public async Task<(List<SteamUser>, int)> GetPaginatedAsync(Expression<Func<SteamUser, bool>> predicate, int page, int pageSize)
        {
            var query = _dbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Where(predicate);

            var total = await query.CountAsync();

            var rows = await query
                .OrderByDescending(i => i.UpdatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (rows, total);
        }

        public async Task<SteamUser?> GetBySteamIdAsync(string steamId) 
            => await _dbSet.FirstOrDefaultAsync(u => u.SteamId == steamId);
    }
}
