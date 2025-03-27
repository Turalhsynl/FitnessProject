namespace Application.CQRS.Carts.ResponseDto;

public class CartDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<CartLineDto> CartLines { get; set; } = [];
    public decimal TotalPrice { get; set; }
}

