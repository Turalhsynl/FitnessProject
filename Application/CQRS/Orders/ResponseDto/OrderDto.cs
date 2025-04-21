namespace Application.CQRS.Orders.ResponseDto;

public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderLineDto> OrderLines { get; set; }
}
