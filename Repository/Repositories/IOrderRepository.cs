using Domain.Entities;

namespace Repository.Repositories;

public interface IOrderRepository
{
    Task<Order> GetOrderByIdAsync(int orderId);
    Task<List<Order>> GetOrdersByUserIdAsync(int userId);
    Task AddAsync(Order order);
}
