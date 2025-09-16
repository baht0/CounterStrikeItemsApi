using CounterStrikeItemsApi.Domain.Models;
using System.Linq.Expressions;

namespace CounterStrikeItemsApi.Domain.Interfaces
{
    public interface IItemCommonRepository : IExtendedRepository<ItemCommon>
    {
        Task<(List<ItemCommon>, int)> GetPaginatedAsync(
            Expression<Func<ItemCommon, bool>> predicate, int page, int pageSize);
    }
}
