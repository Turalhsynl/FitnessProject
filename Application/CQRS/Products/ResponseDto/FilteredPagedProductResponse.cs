namespace Application.CQRS.Products.ResponseDto;

public class FilteredPagedProductResponse
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<GetAllProductDto> Products { get; set; } = new();
}