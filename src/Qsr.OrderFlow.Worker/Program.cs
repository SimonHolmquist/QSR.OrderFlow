using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Qsr.OrderFlow.Infrastructure.Persistence;
using Qsr.OrderFlow.Worker;

var builder = Host.CreateApplicationBuilder(args);

// EF Core (usa tu connection string)
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// Service Bus client
builder.Services.AddSingleton(sp =>
    new ServiceBusClient(builder.Configuration["ServiceBus:Connection"]));

// BackgroundService
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
