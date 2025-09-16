namespace WebAdminPanel.Services.Api
{
    public interface IReferenceApiFactory
    {
        T GetClient<T>() where T : class;
    }
}
