using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qsr.OrderFlow.Application.Orders;
using Qsr.OrderFlow.Infrastructure.Persistence;

namespace Qsr.OrderFlow.Api.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController(IMediator mediator, AppDbContext db) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly AppDbContext _db = db;

    public sealed record CreateOrderRequest(Guid CustomerId, List<OrderItemDto> Items);

    [HttpPost]
    public async Task<ActionResult<object>> Create([FromBody] CreateOrderRequest req)
    {
        var id = await _mediator.Send(new CreateOrder(req.CustomerId, req.Items));
        return CreatedAtAction(nameof(GetById), new { id }, new { orderId = id });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<object>> GetById(Guid id)
    {
        var o = await _db.Orders.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);
        return o is null ? NotFound() : Ok(o);
    }

    [HttpGet]
    public async Task<ActionResult<object>> Search([FromQuery] Guid? customerId, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] bool? paid, [FromQuery] int page = 1, [FromQuery] int size = 20)
    {
        var q = _db.Orders.AsNoTracking();
        if (customerId is not null) q = q.Where(x => x.CustomerId == customerId);
        if (from is not null) q = q.Where(x => x.CreatedOnUtc >= from);
        if (to is not null) q = q.Where(x => x.CreatedOnUtc < to);
        if (paid is not null) q = q.Where(x => x.Paid == paid);
        var items = await q.OrderByDescending(x => x.CreatedOnUtc).Skip((page - 1) * size).Take(size).ToListAsync();
        return Ok(items);
    }
}
