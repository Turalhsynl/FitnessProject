using Domain.Entities;

namespace Repository.Repositories;

public interface ICartLineRepository
{
    Task AddAsync(CartLine cartLine);
    Task<CartLine> GetByIdAsync(int cartLineId);
    Task<IEnumerable<CartLine>> GetByCartIdAsync(int cartId);
    Task UpdateAsync(CartLine cartLine);
    Task RemoveAsync(int cartLineId);
    Task RemoveByCartAndProductAsync(int cartId, int productId);
}
