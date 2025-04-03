using DAL.SqlServer.Context;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace DAL.SqlServer.Infastructure;

public class SqlProductRepository(AppDbContext context) : IProductRepository
{
    private readonly AppDbContext _context = context;
    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }
     //Cart yazilanda olacaq
    public async Task AddToCartAsync(int productId, int userId, int quantity)
    {
        //var cartItem = new CartItem
        //{
        //    ProductId = productId,
        //    UserId = userId,
        //    Quantity = quantity
        //};
        //await _context.CartItems.AddAsync(cartItem);
        //await _context.SaveChangesAsync();
        throw new NotImplementedException();
    }

    //Favorite yazilanda 
    public async Task AddToFavoritesAsync(int productId, int userId)
    {
        //var existingFavorite = await _context.Favorites
        //.FirstOrDefaultAsync(f => f.ProductId == productId && f.UserId == userId);

        //if (existingFavorite == null)
        //{
        //    var favorite = new Favorite
        //    {
        //        ProductId = productId,
        //        UserId = userId
        //    };
        //    await _context.Favorites.AddAsync(favorite);
        //    await _context.SaveChangesAsync();
        //}
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<Product>> SearchPost(string text)
    {
        text = text.ToLower();
        var posts = await _context.Products.Where(x => x.Name.ToLower().Contains(text) || x.Description.ToLower().Contains(text)).ToListAsync();
        return posts;
    }
    public IQueryable<Product> GetAll()
    {
        return _context.Products;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    //Cart yazilanda olacaq
    public async Task<IEnumerable<Product>> GetCartByUserAsync(int userId)
    {
        //var cartItems = await _context.CartItems
        //   .Where(ci => ci.UserId == userId)
        //   .Include(ci => ci.Product)
        //   .ToListAsync();

        //return cartItems.Select(ci => ci.Product);
        throw new NotImplementedException();
    }

    //Favorit olacaq
    public async Task<IEnumerable<Product>> GetFavoritesByUserAsync(int userId)
    {
        //var favorites = await _context.Favorites
        //    .Where(f => f.UserId == userId)
        //    .Include(f => f.Product)
        //    .ToListAsync();

        //return favorites.Select(f => f.Product);
        throw new NotImplementedException();
    }

    public async Task RemoveAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    public async void Update(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> SearchProduct(string text)
    {
        text = text.ToLower(); 
        return await _context.Products
            .Where(x => x.Name.ToLower().Contains(text) || x.Description.ToLower().Contains(text))
            .ToListAsync();
    }

    public IQueryable<Product> GetProductsByPrice(int categoryId, PriceSortOrder sortOrder)
    {
        var query = _context.Products.Where(p => p.CategoryId == categoryId);

        switch (sortOrder)
        {
            case PriceSortOrder.LowToHigh:
                query = query.OrderBy(p => p.Price);
                break;
            case PriceSortOrder.HighToLow:
                query = query.OrderByDescending(p => p.Price);
                break;
        }

        return query;
    }
    public async Task<IEnumerable<Product>> PageProductAsync(int page, int pageSize)
    {
        int skipAmount = (page - 1) * pageSize;
        skipAmount = Math.Max(skipAmount, 0);

        return await _context.Products
            .Skip(skipAmount)
            .Take(pageSize)
            .ToListAsync();
    }

    public IQueryable<Product> GetProductsByColor(ProductColors color)
    {
        return _context.Products.Where(p => p.Color == color);
    }
}
