using CounterStrikeItemsApi.API.Middlewares;
using CounterStrikeItemsApi.Application.DTOs.Reference.Collection;
using CounterStrikeItemsApi.Application.DTOs.Reference.ItemType;
using CounterStrikeItemsApi.Application.DTOs.Reference.Subtype;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Application.Mapping;
using CounterStrikeItemsApi.Application.Services;
using CounterStrikeItemsApi.Application.Services.References;
using CounterStrikeItemsApi.Domain.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using CounterStrikeItemsApi.Infrastructure;
using CounterStrikeItemsApi.Infrastructure.Factories;
using CounterStrikeItemsApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>(optional: true);
        }
        builder.Configuration.AddEnvironmentVariables();

        //Connections
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
            options.InstanceName = "CounterStrikeItemsApi_";
        });

        builder.Services.AddOpenApi();

        #region DI & Mapping
        //DI
        builder.Services.AddSingleton<IPasswordHasher<object>, PasswordHasher<object>>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddScoped<ITokenService, TokenService>();

        builder.Services.AddHttpClient();
        builder.Services.AddScoped<ISteamUserRepository, SteamUserRepository>();
        builder.Services.AddScoped<ISteamUserService, SteamUserService>();

        builder.Services.AddScoped(typeof(IExtendedRepository<>), typeof(ExtendedRepository<>));
        builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        builder.Services.AddScoped(typeof(IReferenceService<,,,>), typeof(ReferenceService<,,,>));
        builder.Services.AddScoped<IItemCommonRepository, ItemCommonRepository>();
        builder.Services.AddScoped<IItemCommonService, ItemCommonService>();

        builder.Services.AddScoped<IReferenceService<CollectionDto, Collection, CollectionCreateDto, CollectionUpdateDto>, CollectionService>();
        builder.Services.AddScoped<IReferenceService<ItemTypeDto, ItemType, ItemTypeCreateDto, ItemTypeUpdateDto>, ItemTypeService>();
        builder.Services.AddScoped<IReferenceService<SubtypeDto, Subtype, SubtypeCreateDto, SubtypeUpdateDto>, SubtypeService>();
        builder.Services.AddScoped<IRepository<Item>, Repository<Item>>();
        builder.Services.AddScoped<ReferenceCacheService>();

        builder.Services.AddScoped<IItemSteamIdUpdater, ItemSteamIdUpdater>();

        //MappingProfiles
        builder.Services.AddAutoMapper(
            typeof(ItemCommonProfile),
            typeof(ItemProfile),
            typeof(ReferenceProfile),
            typeof(ItemTypeProfile),
            typeof(SubtypeProfile));

        //Этот подход говорит сериализатору: "если встретишь цикл — просто не сериализуй заново, игнорируй".
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        #endregion

        #region "Защита"
        // Добавляем CORS политику
        string[] corsOrigins = ["https://localhost:7007", "http://localhost:5155", "http://localhost:5001"];
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AdminPanel", policy =>
            {
                policy.WithOrigins(corsOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        // Rate Limiting
        builder.Services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                if (context.User?.IsInRole("Admin") == true)
                    return RateLimitPartition.GetNoLimiter("Admin");

                // ограничиваем по IP
                var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ip,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });

            options.RejectionStatusCode = 429; // Too Many Requests
        });

        #endregion

        #region Авторизация

        var config = builder.Configuration;
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = config["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddCookie("SteamCookie")
            .AddSteam(options =>
            {
                options.SignInScheme = "SteamCookie"; // Тут должна быть Cookie схема
            });

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

        #endregion

        #region Middlewares

        var app = builder.Build();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        if (app.Environment.IsDevelopment())
            app.MapOpenApi();
        app.UseHttpsRedirection();
        app.UseCors("AdminPanel");
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();

        #endregion
    }
}