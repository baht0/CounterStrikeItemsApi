# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY CounterStrikeItemsApi.sln ./
COPY Workers/ ./Workers/
COPY CounterStrikeItemsApi.Application/ ./CounterStrikeItemsApi.Application/

RUN dotnet restore Workers/Workers.csproj
RUN dotnet publish Workers/Workers.csproj -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Workers.dll"]
