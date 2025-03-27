using Domain.BaseEntities;

namespace Domain.Entities;

public class Cart:BaseEntity
{
    public int Id { get; set; } 
    public int UserId { get; set; }

    public List<CartLine> CartLines { get; set; } = new();

    public decimal TotalPrice => CartLines.Sum(cl => cl.TotalPrice);
}