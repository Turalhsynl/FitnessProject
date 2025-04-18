using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlOrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public SqlOrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
    {
        return await _context.Orders
            .Include(o => o.OrderLines)
            .ThenInclude(ol => ol.Product)
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderLines)
            .ThenInclude(ol => ol.Product)
            .ToListAsync();
    }

    public async Task DeleteAsync(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Order> GetByIdAsync(int orderId)
    {
        return await _context.Orders
                          .Include(o => o.OrderLines)
                          .ThenInclude(ol => ol.Product)
                          .FirstOrDefaultAsync(o => o.Id == orderId);
    }
}
