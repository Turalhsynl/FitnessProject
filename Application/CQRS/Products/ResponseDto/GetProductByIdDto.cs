using Domain.Enums;

namespace Application.CQRS.Products.ResponseDto;

public class GetProductByIdDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public ProductColors Color { get; set; }
    public int CategoryId { get; set; }
}
