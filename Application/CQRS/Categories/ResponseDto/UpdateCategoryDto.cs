namespace Application.CQRS.Categories.ResponseDto;

public class UpdateCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? ImageId { get; set; }
    //public List<Product> Products { get; set; }
}
