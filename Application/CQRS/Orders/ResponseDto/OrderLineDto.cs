namespace Application.CQRS.Orders.ResponseDto;

public class OrderLineDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
