using Domain.BaseEntities;

namespace Domain.Entities;

public class CartLine:BaseEntity
{
    public int CartId { get; set; }
    public Cart? Cart { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int Quantity { get; set; }

    public decimal LineTotal => Product.Price * Quantity;
}
