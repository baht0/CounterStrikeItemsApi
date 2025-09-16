using Refit;
using System.Reflection;
using WebAdminPanel.Contracts.Api.References;
using WebAdminPanel.Services.Auth;

namespace WebAdminPanel.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddRefitClientWithJwt<TClient>(
            this IServiceCollection services,
            string baseUrl)
            where TClient : class
        {
            return services
                .AddRefitClient<TClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl))
                .AddHttpMessageHandler<JwtAuthMessageHandler>();
        }
        public static void AddRefitClientsFromNamespace(
            this IServiceCollection services,
            string @namespace,
            string baseUrl,
            Type? excludeType = null)
        {
            // Используем сборку, где точно лежат интерфейсы
            var assembly = typeof(IBaseReferenceApi).Assembly;

            var refitInterfaces = assembly
                .GetTypes()
                .Where(t =>
                    t.IsInterface &&
                    t.Namespace == @namespace &&
                    t != excludeType &&
                    t.GetMethods().Any(m =>
                        m.GetCustomAttributes().Any(attr =>
                            attr.GetType().Name.StartsWith("Get") || // HttpGetAttribute и др.
                            attr.GetType().Name.StartsWith("Post") ||
                            attr.GetType().Name.StartsWith("Put") ||
                            attr.GetType().Name.StartsWith("Delete")
                        )
                    )
                )
                .ToList();

            foreach (var iface in refitInterfaces)
            {
                try
                {
                    typeof(ServiceCollectionExtensions)
                        .GetMethod(nameof(AddRefitClientWithJwt))!
                        .MakeGenericMethod(iface)
                        .Invoke(null, [services, baseUrl]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to register {iface.Name}: {ex.Message}");
                }
            }
        }
    }
}
