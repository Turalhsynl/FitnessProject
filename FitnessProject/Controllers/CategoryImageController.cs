using Application.Abstractions;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryImageController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;

    [HttpPost("upload-image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProgramImage([FromForm] UploadCategoryImageDto dto)
    {
        var result = await _categoryService.UploadCategoryImageAsync(dto);
        return Ok(new { fileName = result });
    }
}
