using Domain.Entities;

namespace Repository.Repositories;

public interface IOrderLineRepository
{
    Task AddAsync(OrderLine orderLine);
    Task<List<OrderLine>> GetOrderLinesByOrderIdAsync(int orderId);
}
