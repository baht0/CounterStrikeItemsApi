using CounterStrikeItemsApi.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CounterStrikeItemsApi.Infrastructure.Factories
{
    public class RepositoryFactory(IServiceProvider serviceProvider) : IRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public IExtendedRepository<T> GetRepository<T>() where T : class
        {
            var repository = _serviceProvider.GetService<IExtendedRepository<T>>();
            return repository ?? throw new InvalidOperationException($"No repository registered for type {typeof(T).Name}.");
        }
    }
}
