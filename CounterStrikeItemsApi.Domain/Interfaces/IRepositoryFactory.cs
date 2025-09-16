namespace CounterStrikeItemsApi.Domain.Interfaces
{
    public interface IRepositoryFactory
    {
        IExtendedRepository<T> GetRepository<T>() where T : class;
    }
}
