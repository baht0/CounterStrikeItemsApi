# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY CounterStrikeItemsApi.sln ./
COPY WorkerHost/ ./WorkerHost/
COPY Workers/ ./Workers/
COPY CounterStrikeItemsApi.Application/ ./CounterStrikeItemsApi.Application/
COPY CounterStrikeItemsApi.Domain/ ./CounterStrikeItemsApi.Domain/
COPY CounterStrikeItemsApi.Infrastructure/ ./CounterStrikeItemsApi.Infrastructure/

RUN dotnet restore WorkerHost/WorkerHost.csproj
RUN dotnet publish WorkerHost/WorkerHost.csproj -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WorkerHost.dll"]
