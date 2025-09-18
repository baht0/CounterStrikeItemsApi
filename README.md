# CounterStrikeItems
Сервис для централизованного хранения и поиска метаданных о предметах Counter-Strike 2, которые торгуются на площадке SteamCommunity: какие предметы существуют, их качества и категории, источники выпадения и актуальные цены.

## Описание
Проект собирает и структурирует полную информацию о предметах CS2: базовые атрибуты (название, коллекция, тип/подтип, качество), данные о выпадении (источники, турниры, сеты), исторические и текущие цены из интегрируемых источников, а также сопутствующие метаданные. Сервис предоставляет удобный поиск и фильтрацию по ключевым параметрам, авторизацию пользователей через Steam, API для внешних клиентов и админ-панель на Blazor для управления данными. Архитектура рассчитана на надёжность и масштабирование. Цель — дать точную, консистентную и легко доступную базу знаний о предметах CS2.

## Ключевые возможности
* N-слойная архитектура (Domain / Application / Infrastructure / API)
* Авторизация через Steam (OpenID)
* Админ-панель на Blazor WASM с использование MudBlazor
* Авторизация и аутентификации через JWT
* Фоновая обработка задач (WorkerHost)
* База данных PostgreSQL и Redis для кэша
* Полный контейнерный стек (Docker / Docker Compose)

## Структура
* __CounterStrikeItemsApi__                    -> ASP.NET Core 9 Web API (entrypoint)
* __CounterStrikeItemsApi.Application__        -> Сервисы, DTO, интерфейсы, маппинг
* __CounterStrikeItemsApi.Domain__             -> Сущности, интерфейсы
* __CounterStrikeItemsApi.Infrastructure__     -> EF Core, репозитории, фабрики
* __WebAdminPanel__                            -> Админ панель на Blazor WASM
* __WorkerHost__                               -> Хост для фоновых сервисов
* __Workers__                                  -> Background services

## Технологии
* Runtime / Framework: .NET 9 / ASP.NET Core
* Админ панель: Blazor WebAssembly (WASM)
* База данных: PostgreSQL
* Кэширование: Redis
* Контейнеризация: Docker, Docker Compose
* ORM: EF Core

## Быстрый старт (локально)
__1.__ Склонировать репозиторий:
```
git clone https://github.com/baht0/CounterStrikeItemsApi.git
cd CounterStrikeItemsApi
```
__2.__ Выполнить миграцию в директории проекта `*.sln`:
```
dotnet ef migrations add InitialCreate `
  --project CounterStrikeItemsApi.Infrastructure `
  --startup-project CounterStrikeItemsApi

dotnet ef database update `
  --project CounterStrikeItemsApi.Infrastructure `
  --startup-project CounterStrikeItemsApi
```
__3.__ Создать `.env.docker`:
```
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DbConnection=Host=<CHANGE>;Port=<CHANGE>;Database=<CHANGE>;Username=<CHANGE>;Password=<CHANGE>
ConnectionStrings__Redis=redis:6379
AdminCredentials__Username=<CHANGE>
AdminCredentials__Password=<CHANGE>
Jwt__Key=<CHANGE>
Jwt__Issuer=<CHANGE>
Jwt__Audience=<CHANGE>
Steam__ApiKey=<CHANGE>
```
__4.__ Заменить API URL на ваш в `appsettings.json`, `appsettings.Development.json` и `appsettings.Production.json`:
```
{
  "ApiUrl": "https://localhost:5000/api"
}
```
__5.__ Собрать через Docker Compose:
```
docker-compose up -d --build
```

## Сборка
Для `CounterStrikeItemsApi.API` и `WorkerHost` необходимо использовать переменные среды [Secret Manager](https://learn.microsoft.com/ru-ru/aspnet/core/security/app-secrets?view=aspnetcore-9.0&tabs=windows#secret-manager).
### CounterStrikeItemsApi.API:
```
{
  "ASPNETCORE_ENVIRONMENT": "Development",
  "ConnectionStrings:DbConnection": "Host=<CHANGE>;Port=<CHANGE>;Database=<CHANGE>;Username=<CHANGE>;Password=<CHANGE>",
  "ConnectionStrings:Redis": "localhost:6379",
  "Steam:ApiKey": "<CHANGE>",
  "Jwt:Key": "<CHANGE>=",
  "Jwt:Issuer": "<CHANGE>",
  "Jwt:Audience": "<CHANGE>",
  "AdminCredentials:Username": "<CHANGE>",
  "AdminCredentials:Password": "<CHANGE>"
}
```
### WorkerHost:
```
{
  "ASPNETCORE_ENVIRONMENT": "Development",
  "ConnectionStrings:DbConnection": "Host=<CHANGE>;Port=<CHANGE>;Database=<CHANGE>;Username=<CHANGE>;Password=<CHANGE>"
}
```
## Библиотеки / NuGet-зависимости
Дополнительные используемые библиотеки помимо основных:
* __AutoMapper__ — маппинг DTO ↔ сущности, проекция данных в слое приложения.
* __HtmlAgilityPack__ — парсинг/скрейпинг HTML (парсеры страниц с ценами, метаданными и т.п.).
* __AspNet.Security.OpenId.Steam__ — провайдер авторизации через Steam.
* __MudBlazor__ — UI-компоненты для Blazor WASM (админ-панель).
* __Polly__ — устойчивость HTTP-клиентов: retry, circuit-breaker, bulkhead, таймауты (worker host).
* __Refit / Refit.HttpClientFactory__ — декларативные HTTP-клиенты с интеграцией в IHttpClientFactory (админ-панель).
