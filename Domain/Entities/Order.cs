using Domain.BaseEntities;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }

    public List<OrderLine> OrderLines { get; set; }
}

