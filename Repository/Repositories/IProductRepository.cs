using Domain.Entities;

namespace Repository.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product);
    void Update(Product product);
    Task RemoveAsync(int id);
    IQueryable<Product> GetAll();
    Task<Product> GetByIdAsync(int id);
    Task AddToFavoritesAsync(int productId, int userId);
    Task AddToCartAsync(int productId, int userId, int quantity);
    Task<IEnumerable<Product>> GetFavoritesByUserAsync(int userId);
    Task<IEnumerable<Product>> GetCartByUserAsync(int userId);
}
