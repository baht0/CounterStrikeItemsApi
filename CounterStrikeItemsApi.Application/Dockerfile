# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY CounterStrikeItemsApi.sln ./
COPY CounterStrikeItemsApi.Application/ ./CounterStrikeItemsApi.Application/
COPY CounterStrikeItemsApi.Domain/ ./CounterStrikeItemsApi.Domain/

RUN dotnet restore CounterStrikeItemsApi.Application/CounterStrikeItemsApi.Application.csproj
RUN dotnet publish CounterStrikeItemsApi.Application/CounterStrikeItemsApi.Application.csproj -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CounterStrikeItemsApi.Application.dll"]
