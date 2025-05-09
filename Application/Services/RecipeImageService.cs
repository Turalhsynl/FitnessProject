using Application.Abstractions;
using Repository.Common;
using Repository.Repositories;

namespace Application.Services;

public class RecipeImageService(IRecipeRepository recipeRepository, IUnitOfWork unitOfWork, IFileUploadService uploadService) : IRecipeService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileUploadService _uploadService = uploadService;
    private readonly IRecipeRepository _recipeRepository = recipeRepository;

    public async Task<string> UploadRecipeImageAsync(UploadRecipeImageDto dto)
    {
        var fileName = await _uploadService.UploadAsync(dto.RecipeImage);

        var recipe = await _recipeRepository.GetByIdAsync(dto.RecipeId);
        if (recipe == null)
            throw new Exception("recipe not found");

        var file = new Domain.Entities.File
        {
            FileName = dto.RecipeImage.FileName,
            FilePath = fileName,
            FileSize = dto.RecipeImage.Length,
            FileType = dto.RecipeImage.ContentType,
            CreatedDate = DateTime.Now
        };

        await _unitOfWork.FileUploadRepository.AddAsync(file);
        await _unitOfWork.SaveChangeAsync();

        recipe.ImageId = file.Id - 1;
        await _unitOfWork.SaveChangeAsync();

        return fileName;
    }
}
