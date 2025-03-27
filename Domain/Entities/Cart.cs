using Domain.BaseEntities;

namespace Domain.Entities;

public class Cart:BaseEntity
{
    public int UserId { get; set; }

    public List<CartLine> CartLines { get; set; } = new List<CartLine>();

    public decimal TotalPrice => CartLines.Sum(cl => cl.Product.Price * cl.Quantity);
}