# HarbourOps

A port terminal booking platform for managing container logistics services. Customers create bookings for port services (reefer monitoring, weighing, customs handling, priority gate access), submit them for payment, and track fulfillment.

Built with .NET 10, following Domain-Driven Design and Hexagonal Architecture.

## Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│  Driving Adapters                                               │
│  ┌────────────────┐  ┌─────────────────┐                       │
│  │ HarbourOps.Api │  │ HarbourOps.CLI  │                       │
│  └───────┬────────┘  └────────┬────────┘                       │
│          │                    │                                  │
│          ▼                    ▼                                  │
│  ┌─────────────────────────────────────┐                        │
│  │       HarbourOps.Application        │  Use cases / Handlers  │
│  │  (Ports: IBookingRepository,        │                        │
│  │   IPaymentGateway, etc.)            │                        │
│  └───────────────────┬─────────────────┘                        │
│                      │                                           │
│                      ▼                                           │
│  ┌─────────────────────────────────────┐                        │
│  │         HarbourOps.Domain           │  Aggregates, Entities, │
│  │                                     │  Value Objects          │
│  └─────────────────────────────────────┘                        │
│                      ▲                                           │
│                      │                                           │
│  ┌──────────────┐ ┌──────────────────┐ ┌────────────────────┐  │
│  │ EfSqlite     │ │ FakePayments /   │ │ EmailConsole       │  │
│  │ (persistence)│ │ DecliningPayments│ │ (notifications)    │  │
│  └──────────────┘ └──────────────────┘ └────────────────────┘  │
│  Driven Adapters                                                │
└─────────────────────────────────────────────────────────────────┘
```

**Domain** — Pure business logic with no external dependencies. Contains the `Booking` aggregate root (state machine), `ServiceItem`, `BookingLine`, and `Money` value object.

**Application** — Use-case handlers and port interfaces (`IBookingRepository`, `IServiceCatalogRepository`, `IPaymentGateway`, `IBookingConfirmationSender`).

**Adapters** — Swappable implementations of the ports:
- `EfSqlite` — Entity Framework Core + SQLite for persistence
- `FakePayments` / `DecliningPayments` — Test payment gateways
- `EmailConsole` — Prints booking confirmations to stdout

## Booking Lifecycle

```
Draft ──▶ Submitted ──▶ Paid ──▶ Fulfilled
  │                       │
  └──── Cancel ◀──────────┘
```

- **Draft** — Add/remove services, modify booking details
- **Submitted** — Locked for payment (requires at least one service line)
- **Paid** — Payment processed, confirmation sent
- **Fulfilled** — Services delivered (cannot be cancelled)

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Getting Started

```bash
# Clone and build
git clone <repo-url>
cd HarbourOps
dotnet build

# Run the API
dotnet run --project HarbourOps.Api
# Listening on http://localhost:5199

# Run tests
dotnet test
```

The database is created and seeded automatically on first run (SQLite, stored as `harbourops.dev.db`).

## API Endpoints

| Method | Path | Description |
|--------|------|-------------|
| `GET` | `/services` | List available port services |
| `POST` | `/bookings` | Create a new booking |
| `GET` | `/bookings` | List recent bookings |
| `GET` | `/bookings/{id}` | Get booking details |
| `POST` | `/bookings/{id}/services` | Add a service to a booking |
| `POST` | `/bookings/{id}/submit` | Submit booking for payment |
| `POST` | `/bookings/{id}/checkout` | Process payment |
| `POST` | `/bookings/{id}/fulfil` | Mark booking as fulfilled |
| `GET` | `/openapi/v1.json` | OpenAPI specification |

### Example: Create and process a booking

```bash
# Create a booking
curl -X POST http://localhost:5199/bookings \
  -H "Content-Type: application/json" \
  -d '{
    "customerEmail": "ops.manager@example.com",
    "vesselName": "MV Northern Star",
    "containerNumber": "MSCU1234567",
    "requestedDate": "2026-07-01"
  }'

# Add a service (use a service ID from GET /services)
curl -X POST http://localhost:5199/bookings/{id}/services \
  -H "Content-Type: application/json" \
  -d '{"serviceItemId": "...", "quantity": 2}'

# Submit, pay, and fulfil
curl -X POST http://localhost:5199/bookings/{id}/submit
curl -X POST http://localhost:5199/bookings/{id}/checkout
curl -X POST http://localhost:5199/bookings/{id}/fulfil
```

## Service Catalog (Seeded)

| Code | Service | Price |
|------|---------|-------|
| `REEFER-MON` | Reefer Container Monitoring | £125 |
| `VGM-WEIGH` | Verified Gross Mass Weighing | £85 |
| `CUSTOMS-HOLD` | Customs Hold Handling | £210 |
| `PRIORITY-GATE` | Priority Gate Appointment | £60 |

## Project Structure

```
HarbourOps.Domain/              Pure domain model
HarbourOps.Application/         Use cases, handlers, port interfaces
HarbourOps.Api/                 ASP.NET Core REST API
HarbourOps.ImportCli/           CLI tool for batch import
HarbourOps.Adapters.EfSqlite/   EF Core + SQLite persistence
HarbourOps.Adapters.FakePayments/       Always-succeeds payment stub
HarbourOps.Adapters.DecliningPayments/  Always-fails payment stub
HarbourOps.Adapters.EmailConsole/       Console notification adapter
HarbourOps.Tests/               Unit tests (xUnit)
```

## Database Migrations

Migrations run automatically on API startup. To add a new migration:

```bash
dotnet ef migrations add <Name> \
  --project HarbourOps.Adapters.EfSqlite \
  --startup-project HarbourOps.Api
```

## License

[MIT](LICENSE)
