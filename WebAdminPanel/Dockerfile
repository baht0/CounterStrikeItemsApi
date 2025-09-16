# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY CounterStrikeItemsApi.sln ./ 
COPY WebAdminPanel/ ./WebAdminPanel/

RUN dotnet restore WebAdminPanel/WebAdminPanel.csproj
RUN dotnet publish WebAdminPanel/WebAdminPanel.csproj -c Release -o /app/publish

# Stage 2: nginx
FROM nginx:alpine AS final
# удаляем дефолтный конфиг и кладём наш
COPY WebAdminPanel/nginx.conf /etc/nginx/conf.d/default.conf

WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
EXPOSE 80

# стандартная команда nginx в образе уже запускает nginx,
# не нужно переопределять ENTRYPOINT
