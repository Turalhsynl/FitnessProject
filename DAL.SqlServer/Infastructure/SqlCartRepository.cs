using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlCartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public SqlCartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByUserIdAsync(int userId)
    {
        return await _context.Carts
            .Include(c => c.CartLines)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task AddAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cart cart)
    {
        cart.UpdatedDate = DateTime.UtcNow;
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int userId)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null) return false;

        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
        return true;
    }
}