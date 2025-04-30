using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlFavoriteRepository(AppDbContext context) : IFavoriteRepository
{
    private readonly AppDbContext _context = context;

    public bool Add(Favorite favorite)
    {
        _context.Favorites.Add(favorite);
        var result = _context.SaveChanges();
        return result > 0;
    }

    public bool Exists(int userId, int productId)
    {
        return _context.Favorites.Any(f=> f.UserId == userId && f.ProductId == productId);
    }

    public List<Favorite> GetByUserId(int userId)
    {
        return _context.Favorites
            .Include(f => f.Product)
            .Where(f => f.UserId == userId)
            .ToList();
    }

    public bool Remove(int userId, int productId)
    {
        var favorite = _context.Favorites.FirstOrDefault(f=> f.UserId==userId && f.ProductId==productId);
        if (favorite == null)
            return false;
        _context.Favorites.Remove(favorite);
        var result = _context.SaveChanges();
        return result > 0;
    }
}
