using Domain.BaseEntities;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public ICollection<OrderLine> OrderLines { get; set; }
    public decimal TotalPrice { get; set; }
}

