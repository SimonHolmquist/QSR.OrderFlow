using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Qsr.OrderFlow.Application.Common;
using Qsr.OrderFlow.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddDbContext<AppDbContext>(o =>
            o.UseSqlServer(cfg.GetConnectionString("SqlServer")));

        services.AddScoped<IOrderRepository, EfOrderRepository>();
        services.AddScoped<IOutboxRepository, EfOutboxRepository>();

        return services;
    }
}