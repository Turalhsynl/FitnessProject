namespace Application.CQRS.Orders.ResponseDto;

public class OrderLineDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
