using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Qsr.OrderFlow.Infrastructure.Persistence;

var host = new HostBuilder()
    .ConfigureAppConfiguration((ctx, cfg) =>
    {
        cfg.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables();
    })
    .ConfigureServices((ctx, services) =>
    {
        var cs = ctx.Configuration.GetConnectionString("SqlServer")
                 ?? ctx.Configuration["ConnectionStrings:SqlServer"]
                 ?? ctx.Configuration["SqlServer"]; // por las dudas

        services.AddDbContext<AppDbContext>(o =>
            o.UseSqlServer(cs));

        services.AddSingleton(_ =>
            new BlobServiceClient(ctx.Configuration["AzureWebJobsStorage"]));
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
