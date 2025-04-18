using Domain.Entities;

namespace Repository.Repositories;

public interface IOrderRepository
{
    Task AddOrderAsync(Order order);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
    Task<List<Order>> GetAllAsync();
    Task DeleteAsync(int orderId);
    Task<Order> GetByIdAsync(int orderId);
}

