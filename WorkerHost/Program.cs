using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Application.Mapping;
using CounterStrikeItemsApi.Application.Services;
using CounterStrikeItemsApi.Domain.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using CounterStrikeItemsApi.Infrastructure;
using CounterStrikeItemsApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using Workers;

namespace WorkerHost
{
    public class Program
    {
        private static IAsyncPolicy<HttpResponseMessage> RetryPolicy => HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => (int)msg.StatusCode == 429)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        private static IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy => HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        private static IAsyncPolicy<HttpResponseMessage> TimeoutPolicy => Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));

        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                    if (env.IsDevelopment())
                    {
                        config.AddUserSecrets<Program>(optional: true);
                    }
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;
                    services.AddHttpClient("steam", client =>
                    {
                        client.BaseAddress = new Uri("https://steamcommunity.com/");
                        client.Timeout = TimeSpan.FromSeconds(30);
                    })
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                    .AddPolicyHandler(RetryPolicy)
                    .AddPolicyHandler(CircuitBreakerPolicy)
                    .AddPolicyHandler(TimeoutPolicy);
                    services.AddDbContext<AppDbContext>(options => 
                        options.UseNpgsql(configuration.GetConnectionString("DbConnection")));

                    services.AddScoped<IItemSteamIdUpdater, ItemSteamIdUpdater>();
                    services.AddScoped<IRepository<Item>, Repository<Item>>();
                    services.AddHostedService<ItemUpdateWorker>();

                    //MappingProfiles
                    services.AddAutoMapper(
                        typeof(ItemProfile));
                })
                .Build();

            await host.RunAsync();
        }
    }
}