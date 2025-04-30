using Application.CQRS.Products.ResponseDto;
using Domain.Entities;

namespace Application.CQRS.Categories.ResponseDto;

public class GetAllCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public List<GetAllProductDto> Products { get; set; }
}
