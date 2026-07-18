# Restaurants API

Restaurants is a .NET 8 backend API for managing restaurants, dishes, users, and role-based access. Users can browse restaurants, restaurant owners can manage restaurant data and dishes, and administrators can manage user roles.

The solution follows a layered architecture with separate API, Application, Domain, and Infrastructure projects. It uses ASP.NET Core, Entity Framework Core, ASP.NET Core Identity, MediatR, FluentValidation, AutoMapper, Serilog, and SQL Server.

## Features

- ASP.NET Core Identity authentication endpoints.
- Role-based authorization for users, restaurant owners, and administrators.
- Claims- and requirement-based authorization policies.
- Restaurant creation, retrieval, update, deletion, filtering, sorting, and pagination.
- Dish creation, retrieval, and deletion within a restaurant.
- User profile updates.
- Administrator-controlled role assignment and removal.
- Automatic EF Core database migration and seed data on startup.
- Seeded restaurants, dishes, and default application roles.
- Swagger/OpenAPI documentation in development and production.
- Global error handling and request-time logging middleware.
- Structured console and rolling file logging with Serilog.
- API, Application, and Infrastructure test projects.

## Solution Structure

```text
Restaurants.sln
src/
  Restaurants.API/              ASP.NET Core Web API, controllers, middleware, Swagger, auth setup
  Restaurants.Application/      CQRS commands/queries, validators, DTOs, mapping, application services
  Restaurants.Domain/           Entities, constants, repository contracts, authorization contracts, exceptions
  Restaurants.Infrastructure/   EF Core DbContext, migrations, repositories, Identity, authorization, seed data
tests/
  Restaurants.API.Tests/            API controller and middleware tests
  Restaurants.ApplicationTests/     Application handler, validator, mapping, and user-context tests
  Restaurants.InfrastructureTests/  Infrastructure authorization tests
```

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- ASP.NET Core Identity
- Entity Framework Core with SQL Server
- MediatR
- FluentValidation
- AutoMapper
- Serilog
- Swashbuckle / Swagger

## Prerequisites

- .NET SDK 8.0 or later
- SQL Server or SQL Server Express

## Configuration

Configuration is read from `src/Restaurants.API/appsettings.json`, `src/Restaurants.API/appsettings.Development.json`, environment variables, or user secrets.

For local development, prefer user secrets or environment variables for sensitive values:

```powershell
cd src/Restaurants.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:RestaurantsDb" "Server=.\SQLExpress;Database=RestaurantsDb;Trusted_Connection=True;TrustServerCertificate=True"
```

Important configuration keys:

| Section | Key | Purpose |
| --- | --- | --- |
| `ConnectionStrings` | `RestaurantsDb` | SQL Server connection string used by Entity Framework Core. |
| `Serilog` | `WriteTo`, `MinimumLevel` | Console and rolling file logging configuration. |
| `Logging` | `LogLevel` | Default ASP.NET Core logging levels. |
| Root | `AllowedHosts` | Hosts allowed to access the application. |

Do not commit production database credentials or other secrets.

## Getting Started

Restore packages:

```powershell
dotnet restore Restaurants.sln
```

Build the solution:

```powershell
dotnet build Restaurants.sln
```

Run the API:

```powershell
dotnet run --project src/Restaurants.API
```

Default local URLs from `launchSettings.json`:

- HTTP: `http://localhost:5241`
- HTTPS: `https://localhost:7072`
- Swagger: `https://localhost:7072/swagger/index.html`

## Database

The application applies pending Entity Framework Core migrations automatically during startup through `RestaurantSeeder`. It also seeds sample restaurants and dishes along with the default roles:

- `User`
- `Owner`
- `Admin`

To apply migrations manually:

```powershell
dotnet ef database update --project src/Restaurants.Infrastructure --startup-project src/Restaurants.API
```

To add a migration:

```powershell
dotnet ef migrations add MigrationName --project src/Restaurants.Infrastructure --startup-project src/Restaurants.API --output-dir Migrations
```

## API Overview

The API exposes endpoints under the following areas:

| Area | Base Route | Description |
| --- | --- | --- |
| Identity | `/api/identity` | Identity registration, login, account management, user updates, and admin role management. |
| Restaurants | `/api/restaurants` | Restaurant listing, details, creation, updates, and deletion. |
| Dishes | `/api/restaurants/{restaurantId}/dishes` | Dish creation, listing, details, and deletion for a restaurant. |

Use Swagger for complete request and response schemas.

## Authentication and Roles

ASP.NET Core Identity endpoints are mapped under `/api/identity`. Protected endpoints require the access token returned by the login flow:

```http
Authorization: Bearer <access-token>
```

Role-protected operations use the seeded roles:

- `User` for standard application users.
- `Owner` for creating and managing restaurants.
- `Admin` for assigning and removing user roles.

The API also includes authorization policies based on nationality, minimum age, and the number of restaurants created. Restaurant update and deletion operations use resource-based authorization.

## Logging

Serilog writes rolling JSON log files to:

```text
src/Restaurants.API/Logs/
```

Console logging is also enabled. Adjust minimum levels and sinks in the `Serilog` configuration section.

## Testing

Run all tests from the solution root:

```powershell
dotnet test Restaurants.sln
```

The test suite covers API controllers and middleware, application commands and validators, mapping profiles, user context behavior, and infrastructure authorization requirements.

## Development Notes

- Keep business workflows, validation, DTOs, and mapping in `Restaurants.Application`.
- Keep entities, constants, exceptions, and contracts in `Restaurants.Domain`.
- Keep persistence, repositories, Identity, authorization handlers, migrations, and seed data in `Restaurants.Infrastructure`.
- Add HTTP endpoints through controllers in `Restaurants.API`.
- Use MediatR commands and queries for application workflows.
- Add request validation with FluentValidation validators.
- Restrict CORS origins before deploying; the current policy permits any origin, header, and method.
- Disable EF Core sensitive-data logging outside trusted development environments.

## Useful Commands

```powershell
# Restore dependencies
dotnet restore Restaurants.sln

# Build
dotnet build Restaurants.sln

# Run the API
dotnet run --project src/Restaurants.API

# Run all tests
dotnet test Restaurants.sln

# Apply database migrations
dotnet ef database update --project src/Restaurants.Infrastructure --startup-project src/Restaurants.API
```

## Security Notes

- Store connection strings and secrets in user secrets or environment variables.
- Use separate credentials and configuration for each environment.
- Restrict CORS to trusted frontend origins before deployment.
- Use HTTPS in deployed environments.
- Keep EF Core sensitive-data logging disabled in production.

