using Domain.Entities;

namespace Repository.Repositories;

public interface ICartRepository
{
    Task<Cart> GetByIdAsync(int cartId);
    Task<Cart> GetByIdWithProductsAsync(int cartId);
    Task AddAsync(Cart cart);
    Task UpdateAsync(Cart cart);
    Task RemoveProductAsync(int cartId, int productId);
}
