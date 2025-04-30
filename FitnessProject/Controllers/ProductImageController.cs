using Application.Abstractions;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductImageController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpPost("upload-image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProductImage([FromForm] UploadProductImageDto dto)
    {
        var result = await _productService.UploadProductImageAsync(dto);
        return Ok(new { fileName = result });
    }
}
