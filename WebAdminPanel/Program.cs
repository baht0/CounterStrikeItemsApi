using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using WebAdminPanel;
using WebAdminPanel.Contracts.Api.References;
using WebAdminPanel.Mapping;
using WebAdminPanel.Services;
using WebAdminPanel.Services.Api;
using WebAdminPanel.Services.Auth;
using WebAdminPanel.Services.UI;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        var apiUrl = builder.Configuration["ApiUrl"] ?? builder.HostEnvironment.BaseAddress;

        //MudBlazor
        builder.Services.AddMudServices();

        // Регистрация сервисов аутентификации
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<JwtAuthStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
            provider.GetRequiredService<JwtAuthStateProvider>());

        // Регистрация кастомного обработчика
        builder.Services.AddScoped<JwtAuthMessageHandler>();

        // Базовый HttpClient (если нужен отдельно от Refit)
        builder.Services.AddScoped(sp =>
        {
            var handler = sp.GetRequiredService<JwtAuthMessageHandler>();
            handler.InnerHandler = new HttpClientHandler();

            return new HttpClient(handler)
            {
                BaseAddress = new Uri(apiUrl)
            };
        });

        // Регистрация Refit-клиентов
        builder.Services.AddRefitClientsFromNamespace(
            @namespace: "WebAdminPanel.Contracts.Api",
            baseUrl: apiUrl);

        builder.Services.AddRefitClientsFromNamespace(
            @namespace: "WebAdminPanel.Contracts.Api.References",
            baseUrl: apiUrl,
            excludeType: typeof(IBaseReferenceApi));
        builder.Services.AddSingleton<IReferenceApiFactory, ReferenceApiFactory>();

        builder.Services.AddSingleton<ThemeService>(); 

        builder.Services.AddAutoMapper(
            typeof(ItemCommonProfile),
            typeof(ItemProfile),
            typeof(ReferenceProfile));

        await builder.Build().RunAsync();
    }
}