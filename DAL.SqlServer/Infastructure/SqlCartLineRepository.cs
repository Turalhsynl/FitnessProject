using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlCartLineRepository : ICartLineRepository
{
    private readonly AppDbContext _context;

    public SqlCartLineRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CartLine?> GetByIdAsync(int cartLineId)
    {
        return await _context.CartLines.FirstOrDefaultAsync(cl => cl.Id == cartLineId);
    }

    public async Task AddAsync(CartLine cartLine)
    {
        await _context.CartLines.AddAsync(cartLine);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CartLine cartLine)
    {
        _context.CartLines.Update(cartLine);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int cartLineId)
    {
        var cartLine = await _context.CartLines.FirstOrDefaultAsync(cl => cl.Id == cartLineId);
        if (cartLine == null) return false;

        _context.CartLines.Remove(cartLine);
        await _context.SaveChangesAsync();
        return true;
    }
}