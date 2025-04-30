using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlCartRepository(AppDbContext context) : ICartRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Cart> GetByIdAsync(int cartId)
    {
        return await _context.Carts.FindAsync(cartId);
    }

    public async Task<Cart> GetByIdWithProductsAsync(int cartId)
    {
        return await _context.Carts
            .Include(c => c.CartLines)
            .ThenInclude(cl => cl.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId);
    }

    public async Task AddAsync(Cart cart)
    {
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cart cart)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveProductAsync(int cartId, int productId)
    {
        var cartLine = await _context.CartLines
            .FirstOrDefaultAsync(cl => cl.CartId == cartId && cl.ProductId == productId);

        if (cartLine != null)
        {
            _context.CartLines.Remove(cartLine);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Cart> GetCartByUserIdAsync(int userId)
    {
        return await _context.Carts
            .Include(c => c.CartLines)
            .ThenInclude(cl => cl.Product) // Əgər Product lazım deyilsə, bu sətri silə bilərsən
            .FirstOrDefaultAsync(cart => cart.UserId == userId && !cart.IsDeleted);
    }


}