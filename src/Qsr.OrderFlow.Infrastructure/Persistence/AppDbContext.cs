using Microsoft.EntityFrameworkCore;
using Qsr.OrderFlow.Domain.Orders;
using Qsr.OrderFlow.Domain.Outbox;

namespace Qsr.OrderFlow.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OutboxMessage> Outbox => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var order = modelBuilder.Entity<Order>();
        order.ToTable("Orders");
        order.HasKey(x => x.Id);
        order.Property(x => x.Total).HasColumnType("decimal(18,2)");

        order.OwnsMany(o => o.Items, b =>
        {
            b.ToTable("OrderItems");
            b.WithOwner().HasForeignKey("OrderId");
            b.HasKey("OrderId", "ProductId");
            b.Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");
        });

        var outbox = modelBuilder.Entity<OutboxMessage>();
        outbox.ToTable("OutboxMessages");
        outbox.HasKey(x => x.Id);
        outbox.Property(x => x.Type).HasMaxLength(200);
    }
}
