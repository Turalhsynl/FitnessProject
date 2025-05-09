using Application.Abstractions;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecipeImageController(IRecipeService recipeService) : ControllerBase
{
    private readonly IRecipeService _recipeService = recipeService;

    [HttpPost("upload-image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadRecipeImage([FromForm] UploadRecipeImageDto dto)
    {
        var result = await _recipeService.UploadRecipeImageAsync(dto);
        return Ok(new { fileName = result });
    }
}
