namespace WebAdminPanel.Services.Api
{
    public class ReferenceApiFactory(IServiceProvider provider) : IReferenceApiFactory
    {
        private readonly IServiceProvider _provider = provider;

        public T GetClient<T>() where T : class 
            => _provider.GetRequiredService<T>();
    }
}
