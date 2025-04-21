using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlOrderLineRepository(AppDbContext context) : IOrderLineRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(OrderLine orderLine)
    {
        await _context.OrderLines.AddAsync(orderLine);
    }

    public async Task<List<OrderLine>> GetOrderLinesByOrderIdAsync(int orderId)
    {
        return await _context.OrderLines
            .Where(ol => ol.OrderId == orderId)
            .ToListAsync();
    }
}
