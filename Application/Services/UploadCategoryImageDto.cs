using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class UploadCategoryImageDto
{
    public IFormFile CategoryImage { get; set; }
    public int CategoryId { get; set; }
}
