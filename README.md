# CounterStrikeItems
Сервис для централизованного хранения и поиска метаданных о предметах Counter-Strike 2, которые торгуются на площадке SteamCommunity: какие предметы существуют, их качества и категории, источники выпадения и актуальные цены.

## Описание
Проект собирает и структурирует полную информацию о предметах CS2: базовые атрибуты (название, коллекция, тип/подтип, качество), данные о выпадении (источники, турниры, сеты), исторические и текущие цены из интегрируемых источников, а также сопутствующие метаданные. Сервис предоставляет удобный поиск и фильтрацию по ключевым параметрам, API для внешних клиентов и админ-панель на Blazor для управления данными. Архитектура рассчитана на надёжность и масштабирование. Цель — дать точную, консистентную и легко доступную базу знаний о предметах CS2.

## Технологии
* Runtime / Framework: .NET 9 / ASP.NET Core
* Админ панель: Blazor WebAssembly
* База данных: PostgreSQL
* Кэширование: Redis
* Контейнеризация: Docker, Docker Compose
* ORM: EF Core

## Быстрый старт (локально)
1. Склонировать репозиторий:
```
git clone https://github.com/baht0/CounterStrikeItemsApi.git
cd CounterStrikeItemsApi
```
2. Создать .env.docker:
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
