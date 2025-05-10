using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class UploadRecipeImageDto
{
    public IFormFile RecipeImage { get; set; }
    public int RecipeId { get; set; }
}
