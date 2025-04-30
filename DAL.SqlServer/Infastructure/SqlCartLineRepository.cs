using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlCartLineRepository(AppDbContext context) : ICartLineRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(CartLine cartLine)
    {
        _context.CartLines.Add(cartLine);
        await _context.SaveChangesAsync();
    }

    public async Task<CartLine> GetByIdAsync(int cartLineId)
    {
        return await _context.CartLines.FindAsync(cartLineId);
    }

    public async Task<IEnumerable<CartLine>> GetByCartIdAsync(int cartId)
    {
        return await _context.CartLines
            .Where(cl => cl.CartId == cartId)
            .Include(cl => cl.Product)
            .ToListAsync();
    }

    public async Task UpdateAsync(CartLine cartLine)
    {
        _context.CartLines.Update(cartLine);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int cartLineId)
    {
        var cartLine = await _context.CartLines.FindAsync(cartLineId);
        if (cartLine != null)
        {
            _context.CartLines.Remove(cartLine);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveByCartAndProductAsync(int cartId, int productId)
    {
        var cartLine = await _context.CartLines
            .FirstOrDefaultAsync(cl => cl.CartId == cartId && cl.ProductId == productId);

        if (cartLine != null)
        {
            _context.CartLines.Remove(cartLine);
            await _context.SaveChangesAsync();
        }
    }
}
