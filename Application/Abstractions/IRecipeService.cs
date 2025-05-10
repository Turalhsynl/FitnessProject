using Application.Services;

namespace Application.Abstractions;

public interface IRecipeService
{
    Task<string> UploadRecipeImageAsync(UploadRecipeImageDto dto);

}
