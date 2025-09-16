# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY CounterStrikeItemsApi.sln ./
COPY CounterStrikeItemsApi.Infrastructure/ ./CounterStrikeItemsApi.Infrastructure/
COPY CounterStrikeItemsApi.Domain/ ./CounterStrikeItemsApi.Domain/

RUN dotnet restore CounterStrikeItemsApi.Infrastructure/CounterStrikeItemsApi.Infrastructure.csproj
RUN dotnet publish CounterStrikeItemsApi.Infrastructure/CounterStrikeItemsApi.Infrastructure.csproj -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CounterStrikeItemsApi.Infrastructure.dll"]
