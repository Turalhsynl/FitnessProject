using Domain.Entities;

namespace Repository.Repositories;

public interface ICartLineRepository
{
    Task<CartLine?> GetByIdAsync(int cartLineId);
    Task AddAsync(CartLine cartLine);
    Task UpdateAsync(CartLine cartLine);
    Task<bool> DeleteAsync(int cartLineId);
}