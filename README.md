# RetailPlatform

Cart management API built with ASP.NET Core, Redis, and Keycloak for authentication. Runs entirely via Docker Compose.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Postman](https://www.postman.com/downloads/) for running the API collection
- [.NET 10 SDK](https://dotnet.microsoft.com/download) — only needed for running tests locally

Ports that must be free: `50000`, `50001`, `6379`, `8080`.

## Running

From the solution root:

```powershell
docker-compose up --build
```

First startup takes 1–2 minutes while Keycloak imports the realm. To stop:

```powershell
docker-compose down            # stop containers, keep data
docker-compose down -v         # stop containers, wipe all data
```

## URLs and credentials

| Service         | URL                                    | Credentials           |
|-----------------|----------------------------------------|-----------------------|
| API (Scalar UI) | http://localhost:50000/scalar/v1       | —                     |
| API health      | http://localhost:50000/health/ready    | —                     |
| Keycloak admin  | http://localhost:8080                  | `admin` / `admin`     |

Pre-seeded Keycloak users for API access:

| Username    | Password    | Roles           |
|-------------|-------------|-----------------|
| `testuser`  | `Test123!`  | `user`          |
| `adminuser` | `Admin123!` | `user`, `admin` |

## Running requests via Postman

1. Open Postman → **Import** and drop in both files from the repo root:
   - `RetailPlatform.postman_collection.json`
   - `RetailPlatform.Local.postman_environment.json`
2. In the top-right environment selector, pick **RetailPlatform Local**
3. Hit **Send** on any request

The collection auto-fetches a Keycloak token on the first call and caches it until just before expiry — no manual token handling needed. To test as admin, change `keycloak_username` to `adminuser` and `keycloak_password` to `Admin123!` in the environment, then clear `access_token` so a fresh one is fetched.

### Endpoints

All cart endpoints require a bearer token and are under `/api/cart`.

| Method | Route                                   | Purpose              |
|--------|-----------------------------------------|----------------------|
| GET    | `/api/cart/{userId}`                    | Get cart             |
| POST   | `/api/cart/{userId}/items`              | Add item             |
| PUT    | `/api/cart/{userId}/items/{productId}`  | Update item quantity |
| DELETE | `/api/cart/{userId}/items/{productId}`  | Remove item          |
| DELETE | `/api/cart/{userId}`                    | Clear entire cart    |

## Running tests

```powershell
dotnet test
```

## Project structure

```
RetailPlatform/
├── docker-compose.yml
├── RetailPlatform.postman_collection.json
├── RetailPlatform.Local.postman_environment.json
├── keycloak/
│   └── realm-export.json
├── src/
│   ├── Modules/
│   │   └── Carts/
│   │       ├── RetailPlatform.Carts.Application/    # Application services, DTOs
│   │       ├── RetailPlatform.Carts.Domain/         # Domain entities and interfaces
│   │       └── RetailPlatform.Carts.Infrastructure/ # Redis repository
│   └── RetailPlatform.API/                          # Web API host
└── tests/
    └── RetailPlatform.Carts.Domain.Tests/           # Unit tests
```
