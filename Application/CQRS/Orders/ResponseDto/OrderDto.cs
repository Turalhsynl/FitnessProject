namespace Application.CQRS.Orders.ResponseDto;

public class OrderDto
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderLineDto> OrderLines { get; set; }
}
