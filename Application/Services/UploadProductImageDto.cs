using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class UploadProductImageDto
{
    public IFormFile ProductImage { get; set; }
    public int ProductId { get; set; }
}