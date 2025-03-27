using Domain.Entities;

namespace Repository.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(int userId);
    Task AddAsync(Cart cart);
    Task UpdateAsync(Cart cart);
    Task<bool> DeleteAsync(int userId);
}
