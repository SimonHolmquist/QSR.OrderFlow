# Qsr.OrderFlow

Mini system for QSR OrderFlow (Clean Architecture, CQRS/MediatR, Azure Functions, Service Bus, EF Core, OpenTelemetry).

## Quickstart
1. Install .NET 8 SDK.
2. Update **connection strings** in src/Qsr.OrderFlow.Api/appsettings.Development.json and src/Qsr.OrderFlow.Functions/local.settings.json.
3. Build & test:
```bash
dotnet build
dotnet test
```
4. Run API:
```bash
dotnet run --project src/Qsr.OrderFlow.Api/Qsr.OrderFlow.Api.csproj
```
5. Run Worker:
```bash
dotnet run --project src/Qsr.OrderFlow.Worker/Qsr.OrderFlow.Worker.csproj
```
6. Azure Functions (requires Az Functions Core Tools):
```bash
func start --script-root src/Qsr.OrderFlow.Functions
```

