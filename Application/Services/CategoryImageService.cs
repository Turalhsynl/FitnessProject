using Application.Abstractions;
using Repository.Common;
using Repository.Repositories;

namespace Application.Services;

public class CategoryImageService(IFileUploadService fileUploadService, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : ICategoryService
{
    private readonly IFileUploadService _fileUploadService = fileUploadService;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<string> UploadCategoryImageAsync(UploadCategoryImageDto dto)
    {
        var fileName = await _fileUploadService.UploadAsync(dto.CategoryImage);
        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
        if (category == null)
            throw new Exception("Category not found.");

        var file = new Domain.Entities.File
        {
            FileName = dto.CategoryImage.FileName,
            FilePath = fileName,
            FileSize = dto.CategoryImage.Length,
            FileType = dto.CategoryImage.ContentType,
            CreatedDate = DateTime.Now
        };

        await _unitOfWork.FileUploadRepository.AddAsync(file);
        await _unitOfWork.SaveChangeAsync();

        category.ImageId = file.Id - 1;
        await _unitOfWork.SaveChangeAsync();

        return fileName;
    }
}
