using Domain.Entities;

namespace Repository.Repositories;

public interface ICartRepository
{
    Task<Cart> GetCartByUserIdAsync(int userId);
    Task AddToCartAsync(int userId, CartLine cartLine);
    Task RemoveFromCartAsync(int userId, int productId);
    Task ClearCartAsync(int userId);


}
