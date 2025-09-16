using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CounterStrikeItemsApi.Domain.Interfaces;
using CounterStrikeItemsApi.Domain.Models;

namespace CounterStrikeItemsApi.Infrastructure.Repositories
{
    public class ItemCommonRepository(AppDbContext context) : ExtendedRepository<ItemCommon>(context), IItemCommonRepository
    {
        public async Task<(List<ItemCommon>, int)> GetPaginatedAsync(
            Expression<Func<ItemCommon, bool>> predicate, 
            int page, 
            int pageSize)
        {
            var query = _dbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Include(c => c.Collection!)
                    .ThenInclude(ct => ct.Type)
                .Include(i => i.Type)
                .Include(i => i.Subtype)
                .Include(i => i.Items)
                    .ThenInclude(c => c.Category)
                .Include(i => i.Items)
                    .ThenInclude(q => q.Quality)
                .Include(i => i.Items)
                    .ThenInclude(e => e.Exterior)
                .Include(i => i.Items)
                    .ThenInclude(e => e.GraffitiColor)
                .Include(i => i.FoundsAsItem)
                    .ThenInclude(f => f.Container)
                .Where(predicate);

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(i => i.Type)
                    .ThenBy(i => i.Subtype)
                    .ThenBy(i => i.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
